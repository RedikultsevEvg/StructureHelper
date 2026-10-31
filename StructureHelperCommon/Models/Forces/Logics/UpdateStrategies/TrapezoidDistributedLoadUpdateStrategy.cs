using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces
{
    public class TrapezoidDistributedLoadUpdateStrategy : IParentUpdateStrategy<ITrapezoidDistributedLoad>
    {
        private IUpdateStrategy<IBeamSpanLoad> baseUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

        private IUpdateStrategy<IBeamSpanLoad> BaseUpdateStrategy => baseUpdateStrategy ??= new BeamShearLoadBaseUpdateStrategy();
        public void Update(ITrapezoidDistributedLoad targetObject, ITrapezoidDistributedLoad sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            BaseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.StartCoordinate = sourceObject.StartCoordinate;
            targetObject.EndCoordinate = sourceObject.EndCoordinate;
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(sourceObject.StartLoadValue);
                CheckObject.ThrowIfNull(sourceObject.EndLoadValue);
                targetObject.StartLoadValue = sourceObject.StartLoadValue.Clone() as IForceTuple;
                targetObject.EndLoadValue = sourceObject.EndLoadValue.Clone() as IForceTuple;
            }
        }
    }
}
