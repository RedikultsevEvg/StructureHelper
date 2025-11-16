using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupBaseUpdateStrategy : IParentUpdateStrategy<IStirrup>
    {
        private IUpdateStrategy<IPrimitiveVisualProperty> visualUpdateStrategy;
        public bool UpdateChildren { get; set; } = true;

        public void Update(IStirrup targetObject, IStirrup sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.CompressedGap = sourceObject.CompressedGap;
            if (UpdateChildren == true)
            {
                UpdateTargetChildren(targetObject, sourceObject);
            }
        }

        private void UpdateTargetChildren(IStirrup targetObject, IStirrup sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject.VisualProperty);
            CheckObject.ThrowIfNull(targetObject.VisualProperty);
            visualUpdateStrategy ??= new PrimitiveVisualPropertyUpdateStrategy();
            visualUpdateStrategy.Update(targetObject.VisualProperty, sourceObject.VisualProperty);
        }
    }
}
