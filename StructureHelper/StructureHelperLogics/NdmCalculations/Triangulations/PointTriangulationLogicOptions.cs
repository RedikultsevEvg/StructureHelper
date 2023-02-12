using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Primitives;
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
        /// <inheritdoc />
        public double PrestrainKx { get; }
        /// <inheritdoc />
        public double PrestrainKy { get; }
        /// <inheritdoc />
        public double PrestrainEpsZ { get; }

        public PointTriangulationLogicOptions(IPoint2D center, double area)
        {
            Center = center;
            Area = area;
        }

        public PointTriangulationLogicOptions(IPointPrimitive primitive)
        {
            Center = new Point2D() { X = primitive.CenterX, Y = primitive.CenterY };
            Area = primitive.Area;
            PrestrainKx = primitive.UsersPrestrain.Kx + primitive.AutoPrestrain.Kx;
            PrestrainKy = primitive.UsersPrestrain.Ky + primitive.AutoPrestrain.Ky;
            PrestrainEpsZ = primitive.UsersPrestrain.EpsZ + primitive.AutoPrestrain.EpsZ;
        }
    }
}
