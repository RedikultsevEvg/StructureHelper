using LoaderCalculator;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class CalculatorCloningStrategy : ICloneStrategy<ICalculator>
    {
        private ICloningStrategy cloningStrategy;
        private ICalculatorCloningStrategyContainer cloningStrategyContainer;
        private ICalculatorCloningStrategyContainer CloningStrategyContainer => cloningStrategyContainer ??= new CalculatorCloningStrategyContainer(cloningStrategy);

        public CalculatorCloningStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }


        public ICalculator GetClone(ICalculator sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            //to do Change to cloning strategy
            var newCalculator = cloningStrategy.Clone(sourceObject);
            //var newCalculator = calculator.Clone() as ICalculator;
            if (sourceObject is IForceCalculator forceCalculator)
            {
                CloningStrategyContainer.ForceCalculatorStrategy.Update(newCalculator as IForceCalculator, forceCalculator);
            }
            else if (sourceObject is ICrackCalculator crackCalculator)
            {
                CloningStrategyContainer.CrackCalculatorStrategy.Update(newCalculator as ICrackCalculator, crackCalculator);
            }
            else if (sourceObject is ILimitCurvesCalculator limitCalculator)
            {
                CloningStrategyContainer.LimitCurvesCalculatorStrategy.Update(newCalculator as ILimitCurvesCalculator, limitCalculator);
            }
            else if (sourceObject is IValueDiagramCalculator valueDiagramCalculator)
            {
                CloningStrategyContainer.ValueDiagramCalculatorStrategy.Update(newCalculator as IValueDiagramCalculator, valueDiagramCalculator);
            }
            else if (sourceObject is ICurvatureCalculator curvatureCalculator)
            {
                CloningStrategyContainer.CurvatureCalculatorStrategy.Update(newCalculator as ICurvatureCalculator, curvatureCalculator);
            }
            else if (sourceObject is IHistoryCalculator historyCalculator)
            {
                CloningStrategyContainer.HistoryCalculatorStrategy.Update(newCalculator as IHistoryCalculator, historyCalculator);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
            return newCalculator;
        }
    }
}
