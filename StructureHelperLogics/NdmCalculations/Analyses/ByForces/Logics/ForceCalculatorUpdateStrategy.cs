using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    public class ForceCalculatorUpdateStrategy : IUpdateStrategy<IForceCalculator>
    {
        private readonly IUpdateStrategy<IForceCalculatorInputData> inputDataUpdateStrategy;
        public ForceCalculatorUpdateStrategy(IUpdateStrategy<IForceCalculatorInputData> inputDataUpdateStrategy)
        {
            this.inputDataUpdateStrategy = inputDataUpdateStrategy;
        }
        public ForceCalculatorUpdateStrategy() : this(new ForceCalculatorInputDataUpdateStrategy()) {        }
        public void Update(IForceCalculator targetObject, IForceCalculator sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.InputData ??= new ForceCalculatorInputData();
            inputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
        }
    }
}
