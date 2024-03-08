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
    public class RectanglePrimitive : IRectanglePrimitive
    {
        readonly RectangleUpdateStrategy updateStrategy = new();
        public Guid Id { get;}
        public string Name { get; set; }
        public IHeadMaterial? HeadMaterial { get; set; }
        public StrainTuple UsersPrestrain { get; private set; }
        public StrainTuple AutoPrestrain { get; private set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Angle { get; set; }
        public bool ClearUnderlying { get; set; }
        public bool Triangulate { get; set; }
        public IVisualProperty VisualProperty { get; }
        public ICrossSection? CrossSection { get; set; }

        public IPoint2D Center { get; private set; }

        public RectanglePrimitive(Guid id)
        {
            Id = id;
            Name = "New Rectangle";
            NdmMaxSize = 0.01d;
            NdmMinDivision = 10;
            Center = new Point2D();
            VisualProperty = new VisualProperty { Opacity = 0.8d};
            UsersPrestrain = new StrainTuple();
            AutoPrestrain = new StrainTuple();
            ClearUnderlying = false;
            Triangulate = true;
        }
        public RectanglePrimitive() : this(Guid.NewGuid())
        {
                
        }

        public RectanglePrimitive(IHeadMaterial material) : this() { HeadMaterial = material; }

        public object Clone()
        {
            var primitive = new RectanglePrimitive();
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

        public List<NamedValue<IPoint2D>> GetValuePoints()
        {
            var points = new List<NamedValue<IPoint2D>>();
            NamedValue<IPoint2D> newPoint;
            newPoint = new NamedValue<IPoint2D>()
            {
                Name = "Center",
                Value = Center.Clone() as Point2D
            };
            points.Add(newPoint);
            newPoint = new NamedValue<IPoint2D>()
            {
                Name = "LeftTop",
                Value = new Point2D() { X = Center.X - Width / 2d, Y = Center.Y + Height / 2d}
            };
            points.Add(newPoint);
            return points;
        }
    }
}
