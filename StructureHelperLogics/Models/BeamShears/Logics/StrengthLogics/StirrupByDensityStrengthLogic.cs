using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears.Logics;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class StirrupByDensityStrengthLogic : IBeamShearStrenghLogic
    {
        private readonly IStirrupEffectiveness stirrupEffectiveness;
        private readonly IStirrupByDensity stirrupByDensity;
        private readonly IInclinedSection inclinedSection;


        public StirrupByDensityStrengthLogic(
            IStirrupEffectiveness stirrupEffectiveness,
            IStirrupByDensity stirrupByDensity,
            IInclinedSection inclinedSection,
            IShiftTraceLogger? traceLogger)
        {
            this.stirrupEffectiveness = stirrupEffectiveness;
            this.stirrupByDensity = stirrupByDensity;
            this.inclinedSection = inclinedSection;
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }
        /// <inheritdoc/>
        public double GetShearStrength()
        {
            Check();
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Calculation has been started", TraceLogStatuses.Debug);
            if (stirrupByDensity.EndCoordinate < inclinedSection.StartCoord)
            {
                TraceLogger?.AddMessage($"Stirrup end coordinate Xend = {stirrupByDensity.EndCoordinate}(m) is less than incline section start coordinate Xstart = {inclinedSection.StartCoord}(m), stirrup {stirrupByDensity.Name} has been ignored");
                return 0;
            }
            if (stirrupByDensity.StartCoordinate > inclinedSection.EndCoord)
            {
                TraceLogger?.AddMessage($"Stirrup start coordinate Xstart = {stirrupByDensity.StartCoordinate}(m) is bigger than incline section end coordinate Xend = {inclinedSection.EndCoord}(m), stirrup {stirrupByDensity.Name} has been ignored");
                return 0;
            }
            double crackLength = inclinedSection.EndCoord - inclinedSection.StartCoord;
            TraceLogger?.AddMessage($"Length of crack = {inclinedSection.EndCoord} - {inclinedSection.StartCoord} = {crackLength}(m)");
            double crackEndCoord = Math.Min(stirrupByDensity.EndCoordinate, inclinedSection.EndCoord);
            double crackStartCoord = Math.Max(stirrupByDensity.StartCoordinate, inclinedSection.StartCoord);
            double crackLengthViaStirrup = crackEndCoord - crackStartCoord;
            TraceLogger?.AddMessage($"Length of crack via stirrup = {crackEndCoord} - {crackStartCoord} = {crackLengthViaStirrup}(m)");
            double maxCrackLength = stirrupEffectiveness.MaxCrackLengthRatio * inclinedSection.EffectiveDepth;
            TraceLogger?.AddMessage($"Max length of crack = {stirrupEffectiveness.MaxCrackLengthRatio} * {inclinedSection.EffectiveDepth} = {maxCrackLength}(m)");
            double finalCrackLength = Math.Min(crackLengthViaStirrup, maxCrackLength);
            TraceLogger?.AddMessage($"Length of crack = Min({crackLengthViaStirrup}, {maxCrackLength}) = {finalCrackLength}(m)");
            double finalDensity = stirrupEffectiveness.StirrupShapeFactor * stirrupEffectiveness.StirrupPlacementFactor * stirrupByDensity.StirrupDensity;
            TraceLogger?.AddMessage($"Stirrups design density qsw = {stirrupEffectiveness.StirrupShapeFactor} * {stirrupEffectiveness.StirrupPlacementFactor} * {stirrupByDensity.StirrupDensity} = {finalDensity}(N/m)");
            double concreteDensity = inclinedSection.WebWidth * inclinedSection.ConcreteTensionStrength;
            double minFinalDensity = stirrupEffectiveness.MinimumStirrupRatio * concreteDensity;
            if (finalDensity < minFinalDensity)
            {
                TraceLogger?.AddMessage($"Since stirrups design density qsw = {finalDensity}(N/m) less than qsw,min = {stirrupEffectiveness.MinimumStirrupRatio} * {concreteDensity} = {minFinalDensity}(N/m), final density is equal to zero", TraceLogStatuses.Warning);
                finalDensity = 0;
            }
            double strength = finalDensity * finalCrackLength;
            TraceLogger?.AddMessage($"Bearing capacity of stirrups V = {finalDensity} * {finalCrackLength} = {strength}(N)");
            TraceLogger?.AddMessage("Calculation has been finished successfully", TraceLogStatuses.Debug);
            return strength;
        }

        private void Check()
        {
            CheckObject.IsNull(stirrupByDensity);
            CheckObject.IsNull(inclinedSection);
        }
    }
}
