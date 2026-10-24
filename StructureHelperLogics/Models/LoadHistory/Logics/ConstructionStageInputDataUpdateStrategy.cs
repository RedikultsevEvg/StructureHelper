using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class ConstructionStageInputDataUpdateStrategy : IParentUpdateStrategy<IConstructionStageInputData>
    {
        public bool UpdateChildren { get; set; } = true;

        public void Update(IConstructionStageInputData targetObject, IConstructionStageInputData sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.SubStepNumber = sourceObject.SubStepNumber;
            targetObject.Duration = sourceObject.Duration;
            targetObject.ForceAction = sourceObject.ForceAction;
            if (UpdateChildren == true)
            {
                targetObject.Primitives.Clear();
                foreach (var primitive in sourceObject.Primitives)
                {
                    targetObject.Primitives.Add(primitive);
                }
            }
        }
    }
}
