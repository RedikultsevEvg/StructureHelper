using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByRebarStrengthLogic : IBeamShearStrenghLogic
    {
        private IStirrupEffectiveness stirrupEffectiveness;
        private IStirrupByRebar stirrupByRebar;
        private IInclinedSection inclinedSection;
        private readonly IForceTuple forceTuple;
        private StirrupByDensityStrengthLogic stirrupDensityStrengthLogic;
        private IConvertStrategy<IStirrupByDensity, IStirrupByRebar> convertStrategy;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public StirrupByRebarStrengthLogic(
            IStirrupEffectiveness stirrupEffectiveness,
            IStirrupByRebar stirrupByRebar,
            IInclinedSection inclinedSection,
            IForceTuple forceTuple,
            StirrupByDensityStrengthLogic stirrupDensityStrengthLogic,
            IConvertStrategy<IStirrupByDensity, IStirrupByRebar> convertStrategy,
            IShiftTraceLogger? traceLogger)
        {
            this.stirrupEffectiveness = stirrupEffectiveness;
            this.stirrupByRebar = stirrupByRebar;
            this.inclinedSection = inclinedSection;
            this.forceTuple = forceTuple;
            this.stirrupDensityStrengthLogic = stirrupDensityStrengthLogic;
            this.convertStrategy = convertStrategy;
            TraceLogger = traceLogger;
        }

        public StirrupByRebarStrengthLogic(
            IStirrupEffectiveness stirrupEffectiveness,
            IStirrupByRebar stirrupByRebar,
            IInclinedSection inclinedSection,
            IForceTuple forceTuple,
            IShiftTraceLogger? traceLogger)
        {
            this.stirrupEffectiveness = stirrupEffectiveness;
            this.stirrupByRebar = stirrupByRebar;
            this.inclinedSection = inclinedSection;
            this.forceTuple = forceTuple;
            TraceLogger = traceLogger;
        }

        public double GetShearStrength()
        {
            InitializeStrategies();
            TraceLogger?.AddMessage($"Stirrup diameter d = {stirrupByRebar.Diameter}(m)");
            TraceLogger?.AddMessage($"Stirrup leg number n = {stirrupByRebar.LegCount}");
            TraceLogger?.AddMessage($"Stirrup spacing S = {stirrupByRebar.Spacing}(m)");
            double maxSpacingRatio = inclinedSection.ConcreteTensionStrength * inclinedSection.WebWidth * inclinedSection.EffectiveDepth / forceTuple.Qy;
            TraceLogger?.AddMessage($"Maximum spacing ratio due to strength beetwen hoops Sr,max = {maxSpacingRatio}(dimensionless)");
            maxSpacingRatio = Math.Min(maxSpacingRatio, 0.5);
            TraceLogger?.AddMessage($"Maximum spacing ratio Sr,max = {maxSpacingRatio}(dimensionless)");
            double maxStirrupSpacingByEffectibeDepth = maxSpacingRatio * inclinedSection.EffectiveDepth;
            TraceLogger?.AddMessage($"Maximum spacing S,max = {maxStirrupSpacingByEffectibeDepth}(m)");
            if (stirrupByRebar.Spacing > maxStirrupSpacingByEffectibeDepth)
            {
                TraceLogger?.AddMessage($"Stirrup spacing S = {stirrupByRebar.Spacing}(m) is greater than max stirrup spacing Smax = {maxStirrupSpacingByEffectibeDepth}(m), stirrups are ignored", TraceLogStatuses.Warning);
                return 0;
            }
            double shearStrength = stirrupDensityStrengthLogic.GetShearStrength();
            return shearStrength;
        }

        private void InitializeStrategies()
        {
            convertStrategy ??= new StirrupByRebarToDensityConvertStrategy(TraceLogger, inclinedSection);
            IStirrupByDensity stirrupByDensity = convertStrategy.Convert(stirrupByRebar);
            stirrupDensityStrengthLogic ??= new(stirrupEffectiveness, stirrupByDensity, inclinedSection, TraceLogger);
        }
    }
}
