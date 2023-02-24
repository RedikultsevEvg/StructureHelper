using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
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
        public IStrainTuple Prestrain { get; set; }

        /// <inheritdoc />

        public PointTriangulationLogicOptions(IPoint2D center, double area)
        {
            Center = center;
            Area = area;
            Prestrain = new StrainTuple();
        }

        public PointTriangulationLogicOptions(IPointPrimitive primitive)
        {
            Center = new Point2D() { X = primitive.CenterX, Y = primitive.CenterY };
            Area = primitive.Area;
            Prestrain = new StrainTuple
            {
                Kx = primitive.UsersPrestrain.Kx + primitive.AutoPrestrain.Kx,
                Ky = primitive.UsersPrestrain.Ky + primitive.AutoPrestrain.Ky,
                EpsZ = primitive.UsersPrestrain.EpsZ + primitive.AutoPrestrain.EpsZ
            };
        }
    }
}
