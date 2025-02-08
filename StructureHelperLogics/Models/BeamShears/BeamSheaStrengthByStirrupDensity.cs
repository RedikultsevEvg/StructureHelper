using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamSheaStrengthByStirrupDensity : IBeamShearStrenghLogic
    {
        private readonly IStirrupEffectiveness stirrupEffectiveness;
        private readonly IStirrupByDensity stirrupByDensity;
        private readonly IInclinedSection inclinedSection;


        public BeamSheaStrengthByStirrupDensity(
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

        public double GetShearStrength()
        {
            Check();
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Calculation has been started", TraceLogStatuses.Debug);
            double crackLength = inclinedSection.EndCoord - inclinedSection.StartCoord;
            TraceLogger?.AddMessage($"Length of crack = {inclinedSection.EndCoord} - {inclinedSection.StartCoord} = {crackLength}(m)");
            double maxCrackLength = stirrupEffectiveness.MaxCrackLengthFactor * inclinedSection.EffectiveDepth;
            TraceLogger?.AddMessage($"Max length of crack = {stirrupEffectiveness.MaxCrackLengthFactor} * {inclinedSection.EffectiveDepth} = {maxCrackLength}(m)");
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
