using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    public class StirrupByRebarStrengthLogic : IBeamShearStrenghLogic
    {
        private IStirrupEffectiveness stirrupEffectiveness;
        private IStirrupByRebar stirrupByRebar;
        private IInclinedSection inclinedSection;
        private StirrupByDensityStrengthLogic stirrupDensityStrengthLogic;
        private IConvertStrategy<IStirrupByDensity, IStirrupByRebar> convertStrategy;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public StirrupByRebarStrengthLogic(
            IStirrupEffectiveness stirrupEffectiveness,
            IStirrupByRebar stirrupByRebar,
            IInclinedSection inclinedSection,
            StirrupByDensityStrengthLogic stirrupDensityStrengthLogic,
            IConvertStrategy<IStirrupByDensity, IStirrupByRebar> convertStrategy,
            IShiftTraceLogger? traceLogger)
        {
            this.stirrupEffectiveness = stirrupEffectiveness;
            this.stirrupByRebar = stirrupByRebar;
            this.inclinedSection = inclinedSection;
            this.stirrupDensityStrengthLogic = stirrupDensityStrengthLogic;
            this.convertStrategy = convertStrategy;
            TraceLogger = traceLogger;
        }

        public StirrupByRebarStrengthLogic(
            IStirrupEffectiveness stirrupEffectiveness,
            IStirrupByRebar stirrupByRebar,
            IInclinedSection inclinedSection,
            IShiftTraceLogger? traceLogger)
        {
            this.stirrupEffectiveness = stirrupEffectiveness;
            this.stirrupByRebar = stirrupByRebar;
            this.inclinedSection = inclinedSection;
            TraceLogger = traceLogger;
        }

        public double GetShearStrength()
        {
            InitializeStrategies();
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
