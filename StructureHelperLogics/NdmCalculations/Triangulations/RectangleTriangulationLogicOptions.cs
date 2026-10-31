using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <inheritdoc />
    public class RectangleTriangulationLogicOptions : IShapeTriangulationLogicOptions
    {
        private IForceTupleServiceLogic forceTupleServiceLogic;
        private IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();
        /// <inheritdoc />
        public IPoint2D Center { get; set; }
        /// <inheritdoc />
        public double RotationAngle { get; set; } = 0d;
        /// <inheritdoc />
        public IRectangleShape Rectangle { get; }

        /// <inheritdoc />
        public StrainTuple InitialPrestrain { get; set; } = new StrainTuple();
        public ITriangulationOptions TriangulationOptions { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }

        public IDivisionSize DivisionSize { get; } = new DivisionSize(Guid.Empty);
        public IShape Shape { get; set; }
        public StrainTuple HistoryPrestrain { get; set; } = new StrainTuple();

        public RectangleTriangulationLogicOptions(IPoint2D center, IRectangleShape rectangle, double ndmMaxSize, int ndmMinDivision)
        {
            Center = center;
            Rectangle = rectangle;
            DivisionSize.NdmMaxSize = ndmMaxSize;
            DivisionSize.NdmMinDivision = ndmMinDivision;
        }

        public RectangleTriangulationLogicOptions(IRectangleNdmPrimitive primitive)
        {
            Center = new Point2D() {X = primitive.Center.X, Y = primitive.Center.Y };
            RotationAngle = primitive.RotationAngle;
            Rectangle = primitive;
            DivisionSize.NdmMaxSize = primitive.DivisionSize.NdmMaxSize;
            DivisionSize.NdmMinDivision = primitive.DivisionSize.NdmMinDivision;
            HeadMaterial = primitive.NdmElement.HeadMaterial;
            InitialPrestrain = primitive.NdmElement.UsersPrestrain as StrainTuple;
            HistoryPrestrain = ForceTupleServiceLogic.SumTuples(primitive.NdmElement.HistoryPrestrain, primitive.NdmElement.AutoPrestrain) as StrainTuple;
        }
    }
}
