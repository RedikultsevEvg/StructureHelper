using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class ConcentratedForceUpdateStrategy : IUpdateStrategy<IConcentratedForce>
    {
        private IUpdateStrategy<IBeamSpanLoad> baseUpdateStrategy;
        public void Update(IConcentratedForce targetObject, IConcentratedForce sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            baseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.ForceValue = sourceObject.ForceValue.Clone() as IForceTuple;
            targetObject.ForceCoordinate = sourceObject.ForceCoordinate;
        }

        private void InitializeStrategies()
        {
            baseUpdateStrategy ??= new BeamShearLoadBaseUpdateStrategy();
        }
    }
}
