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
        private const double minStirrupRatio = 0.25;
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
            double crackLength = inclinedSection.EndCoord - inclinedSection.StartCoord;
            TraceLogger?.AddMessage($"Length of crack = {inclinedSection.EndCoord} - {inclinedSection.StartCoord} = {crackLength}(m)");
            double maxCrackLength = stirrupEffectiveness.MaxCrackLengthRatio * inclinedSection.EffectiveDepth;
            TraceLogger?.AddMessage($"Max length of crack = {stirrupEffectiveness.MaxCrackLengthRatio} * {inclinedSection.EffectiveDepth} = {maxCrackLength}(m)");
            double finalCrackLength = Math.Min(crackLength, maxCrackLength);
            TraceLogger?.AddMessage($"Length of crack = Min({crackLength}, {maxCrackLength}) = {finalCrackLength}(m)");
            double finalDensity = stirrupEffectiveness.StirrupShapeFactor * stirrupEffectiveness.StirrupPlacementFactor * stirrupByDensity.StirrupDensity;
            TraceLogger?.AddMessage($"Stirrups design density qsw = {finalDensity}(N/m)");
            double concreteDensity = inclinedSection.WebWidth * inclinedSection.ConcreteTensionStrength;
            if (finalDensity < minStirrupRatio * concreteDensity)
            {
                TraceLogger?.AddMessage($"Since stirrups design density qsw = {finalDensity}(N/m) less than {minStirrupRatio} * {concreteDensity}, final density is equal to zero");
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
