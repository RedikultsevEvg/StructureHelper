using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    public class CalculatorCloningStrategyContainer : ICalculatorCloningStrategyContainer
    {
        private ICloningStrategy cloningStrategy;


        public IUpdateStrategy<IForceCalculator> ForceCalculatorStrategy => new ForceCalculatorUpdateCloningStrategy(cloningStrategy);

        public IUpdateStrategy<ICrackCalculator> CrackCalculatorStrategy => new CrackCalculatorUpdateCloningStrategy(cloningStrategy);

        public IUpdateStrategy<ILimitCurvesCalculator> LimitCurvesCalculatorStrategy => new LimitCurvesCalculatorUpdateCloningStrategy(cloningStrategy);

        public IUpdateStrategy<IValueDiagramCalculator> ValueDiagramCalculatorStrategy => new ValueDiagramCalculatorUpdateCloningStrategy(cloningStrategy);
        public CalculatorCloningStrategyContainer(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }
    }
}
