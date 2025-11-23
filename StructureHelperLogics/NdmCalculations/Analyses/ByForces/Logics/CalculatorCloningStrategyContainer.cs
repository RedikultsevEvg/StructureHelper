using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
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
        private IUpdateStrategy<IValueDiagramCalculator> valueDiagramCalculatorStrategy;
        private IUpdateStrategy<ICurvatureCalculator> curvatureCalculatorStrategy;

        public IUpdateStrategy<IForceCalculator> ForceCalculatorStrategy => new ForceCalculatorUpdateCloningStrategy(cloningStrategy);

        public IUpdateStrategy<ICrackCalculator> CrackCalculatorStrategy => new CrackCalculatorUpdateCloningStrategy(cloningStrategy);

        public IUpdateStrategy<ILimitCurvesCalculator> LimitCurvesCalculatorStrategy => new LimitCurvesCalculatorUpdateCloningStrategy(cloningStrategy);

        public IUpdateStrategy<IValueDiagramCalculator> ValueDiagramCalculatorStrategy => valueDiagramCalculatorStrategy ??= new ValueDiagramCalculatorUpdateCloningStrategy(cloningStrategy);
        public IUpdateStrategy<ICurvatureCalculator> CurvatureCalculatorStrategy => curvatureCalculatorStrategy ??= new CurvatureCalculatorUpdateCloningStrategy(cloningStrategy);
        public CalculatorCloningStrategyContainer(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }
    }
}
