using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears.Logics
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
            double maxSpacingRatio = inclinedSection.ConcreteTensionStrength * inclinedSection.WebWidth * inclinedSection.EffectiveDepth / forceTuple.Qy;
            maxSpacingRatio = Math.Min(maxSpacingRatio, 0.5);
            double maxStirrupSpacingByEffectibeDepth = maxSpacingRatio * inclinedSection.EffectiveDepth;
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
            convertStrategy ??= new StirrupByRebarToDensityConvertStrategy(TraceLogger);
            IStirrupByDensity stirrupByDensity = convertStrategy.Convert(stirrupByRebar);
            stirrupDensityStrengthLogic ??= new(stirrupEffectiveness, stirrupByDensity, inclinedSection, TraceLogger);
        }
    }
}
