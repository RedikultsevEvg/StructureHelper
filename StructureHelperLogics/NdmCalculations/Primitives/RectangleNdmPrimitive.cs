using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class RectangleNdmPrimitive : IRectangleNdmPrimitive
    {
        private readonly RectanglePrimitiveUpdateStrategy updateStrategy = new();
        private readonly RectangleShape rectangleShape = new();
        public Guid Id { get;}
        public string Name { get; set; }
        public double Width { get => rectangleShape.Width; set => rectangleShape.Width = value; }
        public double Height { get => rectangleShape.Height; set => rectangleShape.Height = value; }
        public IVisualProperty VisualProperty { get; } = new VisualProperty() { Opacity = 0.8d };
        public ICrossSection? CrossSection { get; set; }

        public IPoint2D Center { get; set; } = new Point2D();

        public INdmElement NdmElement { get; } = new NdmElement();

        public IDivisionSize DivisionSize { get; } = new DivisionSize();

        public IShape Shape => rectangleShape;

        public double RotationAngle { get; set; } = 0d;

        public RectangleNdmPrimitive(Guid id)
        {
            Id = id;
            Name = "New Rectangle";
        }
        public RectangleNdmPrimitive() : this(Guid.NewGuid())
        {
                
        }

        public object Clone()
        {
            var primitive = new RectangleNdmPrimitive();
            updateStrategy.Update(primitive, this);
            return primitive;
        }

        public IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions)
        {
            var ndms = new List<INdm>();
            var options = new RectangleTriangulationLogicOptions(this)
            {
                triangulationOptions = triangulationOptions
            };
            var logic = new RectangleTriangulationLogic(options);
            ndms.AddRange(logic.GetNdmCollection());
            return ndms;
        }

        public bool IsPointInside(IPoint2D point)
        {
            var xMax = Center.X + Width / 2;
            var xMin = Center.X - Width / 2;
            var yMax = Center.Y + Height / 2;
            var yMin = Center.Y - Height / 2;
            if (point.X > xMax ||
                point.X < xMin ||
                point.Y > yMax ||
                point.Y < yMin)
            { return false; }
            return true;
        }

        public List<INamedAreaPoint> GetValuePoints()
        {
            var points = new List<INamedAreaPoint>();
            INamedAreaPoint newPoint;
            newPoint = new NamedAreaPoint()
            {
                Name = "Center",
                Point = Center.Clone() as Point2D
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint()
            {
                Name = "LeftTop",
                Point = new Point2D() { X = Center.X - Width / 2d, Y = Center.Y + Height / 2d}
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint()
            {
                Name = "RightTop",
                Point = new Point2D() { X = Center.X + Width / 2d, Y = Center.Y + Height / 2d }
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint()
            {
                Name = "LeftBottom",
                Point = new Point2D() { X = Center.X - Width / 2d, Y = Center.Y - Height / 2d }
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint()
            {
                Name = "RightBottom",
                Point = new Point2D() { X = Center.X + Width / 2d, Y = Center.Y - Height / 2d }
            };
            points.Add(newPoint);
            return points;
        }
    }
}
