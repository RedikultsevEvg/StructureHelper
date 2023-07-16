using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <summary>
    /// 
    /// </summary>
    public class PointTriangulationLogicOptions : IPointTriangulationLogicOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public IPoint2D Center { get; }
        /// <inheritdoc />
        public double Area { get; }
        public StrainTuple Prestrain { get; set; }

        /// <inheritdoc />

        public PointTriangulationLogicOptions(IPoint2D center, double area)
        {
            Center = center;
            Area = area;
            Prestrain = new StrainTuple();
        }

        public PointTriangulationLogicOptions(IPointPrimitive primitive)
        {
            Center = new Point2D() { X = primitive.Center.X, Y = primitive.Center.Y };
            Area = primitive.Area;
            Prestrain = new StrainTuple
            {
                Mx = primitive.UsersPrestrain.Mx + primitive.AutoPrestrain.Mx,
                My = primitive.UsersPrestrain.My + primitive.AutoPrestrain.My,
                Nz = primitive.UsersPrestrain.Nz + primitive.AutoPrestrain.Nz
            };
        }
    }
}
