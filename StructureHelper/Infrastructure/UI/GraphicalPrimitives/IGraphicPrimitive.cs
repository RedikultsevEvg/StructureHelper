using StructureHelper.Windows.UserControls;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public interface IGraphicalPrimitive
    {
        string Name { get; }
        PrimitiveVisualPropertyViewModel VisualProperty { get; }
    }
}
