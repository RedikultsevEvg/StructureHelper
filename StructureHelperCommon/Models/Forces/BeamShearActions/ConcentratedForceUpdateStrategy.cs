using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class ConcentratedForceUpdateStrategy : IUpdateStrategy<IConcentratedForce>
    {
        private IUpdateStrategy<IBeamShearLoad> baseUpdateStrategy;
        public void Update(IConcentratedForce targetObject, IConcentratedForce sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            baseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.ForceValue = sourceObject.ForceValue;
            targetObject.ForceCoordinate = sourceObject.ForceCoordinate;
        }

        private void InitializeStrategies()
        {
            baseUpdateStrategy ??= new BeamShearLoadBaseUpdateStrategy();
        }
    }
}
