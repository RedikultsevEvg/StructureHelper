using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class BeamShearStrengthByStirrupDensityLogic : IBeamShearStrenghLogic
    {
        private readonly IStirrupEffectiveness stirrupEffectiveness;
        private readonly IStirrupByDensity stirrupByDensity;
        private readonly IInclinedSection inclinedSection;


        public BeamShearStrengthByStirrupDensityLogic(
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
            double strength = stirrupEffectiveness.StirrupShapeFactor * stirrupEffectiveness.StirrupPlacementFactor * finalCrackLength * stirrupByDensity.StirrupDensity;
            TraceLogger?.AddMessage($"Bearing capacity of stirrups V = {stirrupEffectiveness.StirrupShapeFactor} * {stirrupEffectiveness.StirrupPlacementFactor} * {finalCrackLength} * {stirrupByDensity.StirrupDensity} = {strength}(N)");
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
