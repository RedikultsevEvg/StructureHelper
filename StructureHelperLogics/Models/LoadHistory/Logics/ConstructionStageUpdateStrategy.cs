using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class ConstructionStageUpdateStrategy : IParentUpdateStrategy<IConstructionStage>
    {
        private IUpdateStrategy<IConstructionStageInputData> inputDataUpdateStrategy;
        private IUpdateStrategy<IConstructionStageInputData> InputDataUpdateStrategy => inputDataUpdateStrategy ??= new ConstructionStageInputDataUpdateStrategy() { UpdateChildren = UpdateChildren};
        public bool UpdateChildren { get; set; } = true;

        public void Update(IConstructionStage targetObject, IConstructionStage sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            if (UpdateChildren == true)
            {
                InputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
            }
        }
    }
}
