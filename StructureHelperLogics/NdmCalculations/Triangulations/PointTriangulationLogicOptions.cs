using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.NdmPrimitives;
using StructureHelperCommon.Models.Shapes;

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
        public ICenter Center { get; }
        /// <inheritdoc />
        public double Area { get; }
        /// <inheritdoc />
        public double PrestrainKx { get; }
        /// <inheritdoc />
        public double PrestrainKy { get; }
        /// <inheritdoc />
        public double PrestrainEpsZ { get; }

        public PointTriangulationLogicOptions(ICenter center, double area)
        {
            Center = center;
            Area = area;
        }

        public PointTriangulationLogicOptions(INdmPrimitive primitive)
        {
            if (!(primitive.Shape is IPoint)) { throw new StructureHelperException(ErrorStrings.ShapeIsNotCorrect); }
            Center = primitive.Center;
            IPoint point = primitive.Shape as IPoint;
            Center = primitive.Center;
            Area = point.Area;
            PrestrainKx = primitive.PrestrainKx;
            PrestrainKy = primitive.PrestrainKy;
            PrestrainEpsZ = primitive.PrestrainEpsZ;
        }
    }
}
