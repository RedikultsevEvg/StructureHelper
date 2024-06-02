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
        public IPoint2D Center { get; private set; }
        public IHeadMaterial HeadMaterial { get; set; }
        //public double NdmMaxSize { get; set; }
        //public int NdmMinDivision { get; set; }
        public StrainTuple UsersPrestrain { get; private set; }
        public StrainTuple AutoPrestrain { get; private set; }
        public double Area { get; set; }

        public IVisualProperty VisualProperty { get; }
        public bool Triangulate { get; set; }
        public ICrossSection? CrossSection { get; set; }


        public PointPrimitive(Guid id)
        {
            Id = id;
            Name = "New Point";
            Area = 0.0005d;
            Center = new Point2D();
            VisualProperty = new VisualProperty();
            UsersPrestrain = new StrainTuple();
            AutoPrestrain = new StrainTuple();
            Triangulate = true;
        }
        public PointPrimitive() : this (Guid.NewGuid())
        {}
        public PointPrimitive(IHeadMaterial material) : this() { HeadMaterial = material; }

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
