using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class BeamShearActionUpdateStrategy : IParentUpdateStrategy<IBeamShearAction>
    {
        private IUpdateStrategy<IBeamShearAxisAction> axisActionUpdateStrategy;
        private IUpdateStrategy<IBeamShearAxisAction> AxisActionUpdateStrategy => axisActionUpdateStrategy ??= new BeamShearAxisActionUpdateStrategy();

        public bool UpdateChildren { get; set; } = true;

        public void Update(IBeamShearAction targetObject, IBeamShearAction sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.ExternalForce = sourceObject.ExternalForce.GetClone(sourceObject.ExternalForce);
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(sourceObject.SupportAction);
                CheckObject.ThrowIfNull(targetObject.SupportAction);
                AxisActionUpdateStrategy.Update(targetObject.SupportAction, sourceObject.SupportAction);
            }
        }
    }
}
