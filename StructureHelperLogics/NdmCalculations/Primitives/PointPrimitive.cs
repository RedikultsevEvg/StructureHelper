using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.Models.Primitives
{
    public class PointPrimitive : IPointPrimitive
    {
        static readonly PointUpdateStrategy updateStrategy = new();
        public Guid Id { get; }
        public string? Name { get; set; }
        public IPoint2D Center { get; set; }
        public double Area { get; set; }

        public IVisualProperty VisualProperty { get; } = new VisualProperty();

        public ICrossSection? CrossSection { get; set; }

        public INdmElement NdmElement { get; } = new NdmElement();

        public IShape Shape => throw new NotImplementedException();

        public PointPrimitive(Guid id)
        {
            Id = id;
            Name = "New Point";
            Area = 0.0005d;
            Center = new Point2D();
        }
        public PointPrimitive() : this (Guid.NewGuid())
        {}

        public object Clone()
        { 
            var primitive = new PointPrimitive();
            updateStrategy.Update(primitive, this);
            return primitive;
        }

        public IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions)
        {
            var options = new PointTriangulationLogicOptions(this) { triangulationOptions = triangulationOptions};
            var logic = new PointTriangulationLogic(options);
            return logic.GetNdmCollection();
        }

        public List<INamedAreaPoint> GetValuePoints()
        {
            var points = new List<INamedAreaPoint>();
            var newPoint = new NamedAreaPoint()
            {
                Name = "Center",
                Point = Center.Clone() as Point2D,
                Area = Area
            };
            points.Add(newPoint);
            return points;
        }
    }
}
