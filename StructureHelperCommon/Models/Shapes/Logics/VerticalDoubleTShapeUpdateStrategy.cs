using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Shapes
{
    public class VerticalDoubleTShapeUpdateStrategy : IUpdateStrategy<IVerticalDoubleTShape>
    {
        public void Update(IVerticalDoubleTShape targetObject, IVerticalDoubleTShape sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, nameof(targetObject));
            CheckObject.ThrowIfNull(sourceObject, nameof(sourceObject));
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.FullHeight = sourceObject.FullHeight;
            targetObject.WebThickness = sourceObject.WebThickness;
            targetObject.TopFlangeThickness = sourceObject.TopFlangeThickness;
            targetObject.TopFlangeWidth = sourceObject.TopFlangeWidth;
            targetObject.BottomFlangeThickness = sourceObject.BottomFlangeThickness;
            targetObject.BottomFlangeWidth = sourceObject.BottomFlangeWidth;
        }
    }
}
