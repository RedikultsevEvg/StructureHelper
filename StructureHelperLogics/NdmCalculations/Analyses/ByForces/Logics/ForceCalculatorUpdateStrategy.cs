using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Sections;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    public class ForceCalculatorUpdateStrategy : IUpdateStrategy<ForceCalculator>
    {
        private readonly IUpdateStrategy<ForceInputData> inputDataUpdateStrategy;
        public ForceCalculatorUpdateStrategy(IUpdateStrategy<ForceInputData> inputDataUpdateStrategy)
        {
            this.inputDataUpdateStrategy = inputDataUpdateStrategy;
        }
        public ForceCalculatorUpdateStrategy() : this(new ForceCalculatorInputDataUpdateStrategy()) {        }
        public void Update(ForceCalculator targetObject, ForceCalculator sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.InputData ??= new ForceInputData();
            inputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
        }
    }
}
