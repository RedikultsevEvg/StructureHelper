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
    public class CirclePrimitive : ICirclePrimitive
    {
        static readonly CircleUpdateStrategy updateStrategy = new();
        /// <inheritdoc/>
        public Guid Id { get; set; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public IPoint2D Center { get; private set; }
        public IHeadMaterial? HeadMaterial { get; set; }
        public StrainTuple UsersPrestrain { get; }
        public StrainTuple AutoPrestrain { get; }
        public IVisualProperty VisualProperty { get; }
        public double Diameter { get; set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public bool ClearUnderlying { get; set; }
        public bool Triangulate { get; set; }
        public ICrossSection? CrossSection { get; set; }


        public CirclePrimitive(Guid id)
        {
            Id = id;
            Name = "New Circle";
            NdmMaxSize = 0.01d;
            NdmMinDivision = 10;
            Center = new Point2D();
            VisualProperty = new VisualProperty { Opacity = 0.8d };
            UsersPrestrain = new StrainTuple();
            AutoPrestrain = new StrainTuple();
            ClearUnderlying = false;
            Triangulate = true;
        }
        public CirclePrimitive() : this (Guid.NewGuid())
        {}
        /// <inheritdoc/>
        public object Clone()
        {
            var primitive = new CirclePrimitive();
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
            if (distance > Diameter / 2) { return false; }
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
                Point = new Point2D() { X = Center.X - Diameter / 2d, Y = Center.Y},
                Area = 0d
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint
            {
                Name = "Top",
                Point = new Point2D() { X = Center.X, Y = Center.Y + Diameter / 2d },
                Area = 0d
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint
            {
                Name = "Right",
                Point = new Point2D() { X = Center.X + Diameter / 2d, Y = Center.Y },
                Area = 0d
            };
            points.Add(newPoint);
            newPoint = new NamedAreaPoint
            {
                Name = "Bottom",
                Point = new Point2D() { X = Center.X, Y = Center.Y - Diameter / 2d },
                Area = 0d
            };
            points.Add(newPoint);
            return points;
        }
    }
}
