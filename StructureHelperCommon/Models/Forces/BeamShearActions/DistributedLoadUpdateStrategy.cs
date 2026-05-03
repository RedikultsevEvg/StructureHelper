using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class DistributedLoadUpdateStrategy : IParentUpdateStrategy<IDistributedLoad>
    {
        private IUpdateStrategy<IBeamSpanLoad> baseUpdateStrategy;

        private IUpdateStrategy<IBeamSpanLoad> BaseUpdateStrategy => baseUpdateStrategy ??= new BeamShearLoadBaseUpdateStrategy();
        public bool UpdateChildren { get; set; } = true;
        public void Update(IDistributedLoad targetObject, IDistributedLoad sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }

            BaseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.EndCoordinate = sourceObject.EndCoordinate;
            targetObject.StartCoordinate = sourceObject.StartCoordinate;
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(sourceObject.LoadValue);
                targetObject.LoadValue = sourceObject.LoadValue.Clone() as IForceTuple;
            }
        }
    }
}
