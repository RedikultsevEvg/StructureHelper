using StructureHelperCommon.Models.Shapes;
using System.Windows.Media;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public class ShadedPrimitive : IShadedPrimitive
    {
        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public IShape Shape { get; set; }
        public IPoint2D Center { get; set; } = new Point2D();

        public ShadedPrimitive(IShape shape)
        {
            Shape = shape;
        }
    }
}
