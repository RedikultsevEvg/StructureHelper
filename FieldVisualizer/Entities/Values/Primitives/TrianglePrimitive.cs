using StructureHelperCommon.Models.Shapes;
using System;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public class TrianglePrimitive : ITrianglePrimitive
    {
        public double Value { get; set; }

        public IPoint2D Point1 { get; set; }
        public IPoint2D Point2 { get; set; }
        public IPoint2D Point3 { get; set; }

        // --- Computed properties ---

        // Centroid (geometric center)
        public double CenterX => (Point1.X + Point2.X + Point3.X) / 3.0;
        public double CenterY => (Point1.Y + Point2.Y + Point3.Y) / 3.0;

        // Triangle area using determinant formula
        public double Area =>
            0.5 * Math.Abs(
                (Point2.X - Point1.X) * (Point3.Y - Point1.Y) -
                (Point3.X - Point1.X) * (Point2.Y - Point1.Y)
            );

        public double ValuePoint1 { get; set; }
        public double ValuePoint2 { get; set; }
        public double ValuePoint3 { get; set; }
    }
}
