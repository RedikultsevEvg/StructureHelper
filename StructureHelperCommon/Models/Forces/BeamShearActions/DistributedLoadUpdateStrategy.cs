using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class DistributedLoadUpdateStrategy : IUpdateStrategy<IDistributedLoad>
    {
        private IUpdateStrategy<IBeamSpanLoad> baseUpdateStrategy;
        public void Update(IDistributedLoad targetObject, IDistributedLoad sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            baseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.LoadValue = sourceObject.LoadValue.Clone() as IForceTuple;
            targetObject.StartCoordinate = sourceObject.StartCoordinate;
            targetObject.EndCoordinate = sourceObject.EndCoordinate;
        }
        private void InitializeStrategies()
        {
            baseUpdateStrategy ??= new BeamShearLoadBaseUpdateStrategy();
        }
    }
}
