using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <inheritdoc />
    public class RectangleTriangulationLogicOptions : IShapeTriangulationLogicOptions
    {
        /// <inheritdoc />
        public IPoint2D Center { get; }
        /// <inheritdoc />
        public IRectangleShape Rectangle { get; }
        /// <inheritdoc />
        public double NdmMaxSize { get; }
        /// <inheritdoc />
        public int NdmMinDivision { get; }
        /// <inheritdoc />
        public StrainTuple Prestrain { get; set; }
        public ITriangulationOptions triangulationOptions { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }

        public RectangleTriangulationLogicOptions(IPoint2D center, IRectangleShape rectangle, double ndmMaxSize, int ndmMinDivision)
        {
            Center = center;
            Rectangle = rectangle;
            NdmMaxSize = ndmMaxSize;
            NdmMinDivision = ndmMinDivision;
            Prestrain = new StrainTuple();
        }

        public RectangleTriangulationLogicOptions(IRectanglePrimitive primitive)
        {
            Center = new Point2D() {X = primitive.Center.X, Y = primitive.Center.Y };
            Rectangle = primitive;
            NdmMaxSize = primitive.NdmMaxSize;
            NdmMinDivision = primitive.NdmMinDivision;
            HeadMaterial = primitive.HeadMaterial;
            Prestrain = ForceTupleService.SumTuples(primitive.UsersPrestrain, primitive.AutoPrestrain) as StrainTuple;
        }
    }
}
