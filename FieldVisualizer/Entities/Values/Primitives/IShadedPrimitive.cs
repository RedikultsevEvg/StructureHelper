using StructureHelperCommon.Models.Shapes;
using System.Windows.Media;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public interface IShadedPrimitive
    {
        Color BackgroundColor { get; set; }
        Color BorderColor { get; set; }
        IPoint2D Center { get; set; }
        IShape Shape { get; set; }
    }
}
