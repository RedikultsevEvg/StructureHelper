using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class RectangleNdmPrimitive : IRectangleNdmPrimitive
    {
        private RectanglePrimitiveUpdateStrategy updateStrategy;
        private readonly RectangleShape rectangleShape = new();
        public Guid Id { get;}
        public string Name { get; set; }
        public double Width { get => rectangleShape.Width; set => rectangleShape.Width = value; }
        public double Height { get => rectangleShape.Height; set => rectangleShape.Height = value; }
        public IVisualProperty VisualProperty { get; } = new VisualProperty() { Opacity = 0.8d };
        public ICrossSection? CrossSection { get; set; }

        public IPoint2D Center { get; set; } = new Point2D();

        public INdmElement NdmElement { get; } = new NdmElement();

        public IDivisionSize DivisionSize { get; } = new DivisionSize(Guid.NewGuid());

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

        public IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions)
        {
            var ndms = new List<INdm>();
            var options = new RectangleTriangulationLogicOptions(this)
            {
                TriangulationOptions = triangulationOptions
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
                Point = Center.GetClone(Center) as Point2D
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

        public INdmPrimitive GetClone(INdmPrimitive sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            RectangleNdmPrimitive primitive = new (id);
            if (sourceObject is not IRectangleNdmPrimitive sourceRectangle)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
            new RectanglePrimitiveUpdateStrategy().Update(primitive, sourceRectangle);
            return primitive;
        }
    }
}
