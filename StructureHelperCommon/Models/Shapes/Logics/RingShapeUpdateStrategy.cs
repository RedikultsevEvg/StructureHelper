using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Shapes
{
    public class RingShapeUpdateStrategy : IUpdateStrategy<IRingShape>
    {
        public void Update(IRingShape targetObject, IRingShape sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.OuterDiameter = sourceObject.OuterDiameter;
            targetObject.InnerDiameter = sourceObject.InnerDiameter;
        }
    }
}
