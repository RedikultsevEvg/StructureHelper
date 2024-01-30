using LoaderCalculator.Data.Materials;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <summary>
    /// 
    /// </summary>
    public class PointTriangulationLogicOptions : ITriangulationLogicOptions
    {
        public ITriangulationOptions triangulationOptions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IPoint2D Center { get; }
        /// <inheritdoc />
        public double Area { get; }
        public StrainTuple Prestrain { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }

        /// <inheritdoc />

        public PointTriangulationLogicOptions(IPointPrimitive primitive)
        {
            Center = primitive.Center.Clone() as Point2D;
            Area = primitive.Area;
            HeadMaterial = primitive.HeadMaterial;
            Prestrain = ForceTupleService.SumTuples(primitive.UsersPrestrain, primitive.AutoPrestrain) as StrainTuple;
        }
    }
}
