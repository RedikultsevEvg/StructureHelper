using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Shapes
{
    public class VerticalTShapeUpdateStrategy : IUpdateStrategy<IVerticalTShape>
    {
        public void Update(IVerticalTShape targetObject, IVerticalTShape sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.FullHeight = sourceObject.FullHeight;
            targetObject.WebWidth = sourceObject.WebWidth;
            targetObject.FlangeHeight = sourceObject.FlangeHeight;
            targetObject.FlangeWidth = sourceObject.FlangeWidth;
            targetObject.FlangeXOffset = sourceObject.FlangeXOffset;
        }
    }
}
