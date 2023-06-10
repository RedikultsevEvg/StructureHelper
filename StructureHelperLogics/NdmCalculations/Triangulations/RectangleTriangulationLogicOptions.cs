using System;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <inheritdoc />
    public class RectangleTriangulationLogicOptions : IRectangleTriangulationLogicOptions
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
            Center = new Point2D() { X = primitive.CenterX, Y = primitive.CenterY };
            Rectangle = primitive;
            NdmMaxSize = primitive.NdmMaxSize;
            NdmMinDivision = primitive.NdmMinDivision;
            Prestrain = new StrainTuple
            {
                Mx = primitive.UsersPrestrain.Mx + primitive.AutoPrestrain.Mx,
                My = primitive.UsersPrestrain.My + primitive.AutoPrestrain.My,
                Nz = primitive.UsersPrestrain.Nz + primitive.AutoPrestrain.Nz
            };
        }
    }
}
