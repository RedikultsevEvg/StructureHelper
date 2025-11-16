using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class BeamShearActionUpdateStrategy : IUpdateStrategy<IBeamShearAction>
    {
        private IUpdateStrategy<IBeamShearAxisAction> axisActionUpdateStrategy;
        public void Update(IBeamShearAction targetObject, IBeamShearAction sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.ExternalForce = sourceObject.ExternalForce.Clone() as IFactoredForceTuple;
            InitializeStrategies();
            CheckObject.ThrowIfNull(sourceObject.SupportAction);
            CheckObject.ThrowIfNull(targetObject.SupportAction);
            axisActionUpdateStrategy.Update(targetObject.SupportAction, sourceObject.SupportAction);
        }

        private void InitializeStrategies()
        {
            axisActionUpdateStrategy ??= new BeamShearAxisActionUpdateStrategy();
        }
    }
}
