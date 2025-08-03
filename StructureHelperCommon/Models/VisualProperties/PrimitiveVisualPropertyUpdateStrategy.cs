using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.VisualProperties
{
    public class PrimitiveVisualPropertyUpdateStrategy : IUpdateStrategy<IPrimitiveVisualProperty>
    {
        public void Update(IPrimitiveVisualProperty targetObject, IPrimitiveVisualProperty sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.IsVisible = sourceObject.IsVisible;
            targetObject.Color = sourceObject.Color;
            targetObject.Opacity = sourceObject.Opacity;
            targetObject.ZIndex = sourceObject.ZIndex;
        }
    }
}
