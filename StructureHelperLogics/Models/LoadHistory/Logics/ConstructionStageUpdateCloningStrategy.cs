using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class ConstructionStageUpdateCloningStrategy : IUpdateStrategy<IConstructionStage>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IConstructionStageInputData> inputDataUpdateStrategy; 
        private IUpdateStrategy<IConstructionStageInputData> InputDataUpdateStrategy => inputDataUpdateStrategy ??= new ConstructionStageInputDataUpdateCloningStrategy(); 

        public ConstructionStageUpdateCloningStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IConstructionStage targetObject, IConstructionStage sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
        }
    }
}
