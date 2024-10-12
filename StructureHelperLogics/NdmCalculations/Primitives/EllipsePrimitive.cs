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
    public class EllipsePrimitive : IEllipsePrimitive
    {
        private static readonly EllipsePrimitiveUpdateStrategy updateStrategy = new();
        private readonly RectangleShape rectangleShape = new();

        /// <inheritdoc/>
        public Guid Id { get; set; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public IPoint2D Center { get; set; } = new Point2D();
        /// <inheritdoc/>
        public IVisualProperty VisualProperty { get; } = new VisualProperty { Opacity = 0.8d };
        /// <inheritdoc/>
        public double Width
        {
            get
            {
                return rectangleShape.Width;
            }
            set
            {
                rectangleShape.Width = value;
                rectangleShape.Height = value;
            }
        }
        /// <inheritdoc/>
        public double Height { get => rectangleShape.Height; set => rectangleShape.Height = value; }
        /// <inheritdoc/>
        public ICrossSection? CrossSection { get; set; }
        /// <inheritdoc/>
        public INdmElement NdmElement { get; } = new NdmElement();
        /// <inheritdoc/>
        public IDivisionSize DivisionSize { get; } = new DivisionSize();
        /// <inheritdoc/>
        public IShape Shape => rectangleShape;

        public double Angle { get; set; }

        public EllipsePrimitive(Guid id)
        {
            Id = id;
            Name = "New Circle";
        }
        public EllipsePrimitive() : this (Guid.NewGuid())  {}
        /// <inheritdoc/>
        public object Clone()
        {
            var primitive = new EllipsePrimitive();
            updateStrategy.Update(primitive, this);
            return primitive;
        }
        /// <inheritdoc/>
        public IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions)
        {
            var ndms = new List<INdm>();
            var options = new CircleTriangulationLogicOptions(this)
            {
                triangulationOptions = triangulationOptions
            };
            var logic = new CircleTriangulationLogic(options);
            ndms.AddRange(logic.GetNdmCollection());
            return ndms;
        }
        /// <inheritdoc/>
        public bool IsPointInside(IPoint2D point)
        {
            var dX = Center.X - point.X;
            var dY = Center.Y - point.Y;
            var distance = Math.Sqrt(dX * dX + dY * dY);
            if (distance > Width / 2) { return false; }
            return true;
        }

        List<INamedAreaPoint> INdmPrimitive.GetValuePoints()
        {
            var points = new List<INamedAreaPoint>();
            INamedAreaPoint newPoint;
            newPoint = new NamedAreaPoint
            {
                Name = "Center",
                Point = Center.Clone() as Point2D,
                Area = 0d
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint
            {
                Name = "Left",
                Point = new Point2D() { X = Center.X - Width / 2d, Y = Center.Y},
                Area = 0d
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint
            {
                Name = "Top",
                Point = new Point2D() { X = Center.X, Y = Center.Y + Width / 2d },
                Area = 0d
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint
            {
                Name = "Right",
                Point = new Point2D() { X = Center.X + Width / 2d, Y = Center.Y },
                Area = 0d
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint
            {
                Name = "Bottom",
                Point = new Point2D() { X = Center.X, Y = Center.Y - Width / 2d },
                Area = 0d
            };
            points.Add(newPoint);
            return points;
        }
    }
}
