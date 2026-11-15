using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class ShapeNdmPrimitive : IShapeNdmPrimitive
    {
        private IShape shape;
        private IUpdateStrategy<IShapeNdmPrimitive> updateStrategy;

        public Guid Id { get; }
        public string? Name { get; set; } = string.Empty;
        public IPoint2D Center { get; set; } = new Point2D();
        public IShape Shape => shape;
        public INdmElement NdmElement { get; set; } = new NdmElement(Guid.NewGuid());
        public ICrossSectionRepository? CrossSectionRepository { get; set; }
        public IVisualProperty VisualProperty { get; set; } = new VisualProperty { Opacity = 0.8d };
        public double RotationAngle { get; set; }
        public IDivisionSize DivisionSize { get; set; } = new DivisionSize(Guid.NewGuid());
        public ShapeNdmPrimitive(Guid id)
        {
            Id = id;
        }

        public IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions)
        {
            var triangulationLogicOption = new ShapeTriangulationLogicOption(this, triangulationOptions);
            var logic = new ShapeTriangulationLogic(triangulationLogicOption);
            return logic.GetNdmCollection();
        }

        public List<INamedAreaPoint> GetValuePoints()
        {
            var points = new List<INamedAreaPoint>();
            INamedAreaPoint newPoint;
            newPoint = new NamedAreaPoint
            {
                Name = "Center",
                Point = Center.GetClone(Center) as Point2D,
                Area = 0d
            };
            points.Add(newPoint);
            if (shape is ILinePolygonShape polygon)
            {
                int i = 0;
                foreach (var item in polygon.Vertices)
                {
                    newPoint = new NamedAreaPoint
                    {
                        Name = $"Vertex {i}",
                        Point = item.Point.GetClone(item.Point) as IPoint2D,
                        Area = 0d
                    };
                    points.Add(newPoint);
                    i++;
                }
            }
            return points;
        }

        public bool IsPointInside(IPoint2D point)
        {
            var logic = new IsPointInsideShapeLogic();
            return logic.IsPontInside(point, new CenterShape(Center, Shape));
        }

        public void SetShape(IShape shape)
        {
            this.shape = shape;
        }

        public INdmPrimitive GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            var primitive = new ShapeNdmPrimitive(id);
            var logic = new ShapeCloneStrategy();
            IShape polygon = logic.GetClone(Shape, StructureHelperCommon.Infrastructures.Enums.CloneMode.NewEntity);
            primitive.SetShape(polygon);
            updateStrategy ??= new ShapeNdmPrimitiveUpdateStrategy() { UpdateChildren = true };
            updateStrategy.Update(primitive, this);
            return primitive;
        }
    }
}
