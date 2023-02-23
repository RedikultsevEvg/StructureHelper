using System;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
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
        public double PrestrainKx { get;}
        /// <inheritdoc />
        public double PrestrainKy { get; }
        /// <inheritdoc />
        public double PrestrainEpsZ { get;}

        public RectangleTriangulationLogicOptions(IPoint2D center, IRectangleShape rectangle, double ndmMaxSize, int ndmMinDivision)
        {
            Center = center;
            Rectangle = rectangle;
            NdmMaxSize = ndmMaxSize;
            NdmMinDivision = ndmMinDivision;
        }

        public RectangleTriangulationLogicOptions(IRectanglePrimitive primitive)
        {
            Center = new Point2D() { X = primitive.CenterX, Y = primitive.CenterY };
            Rectangle = primitive;
            NdmMaxSize = primitive.NdmMaxSize;
            NdmMinDivision = primitive.NdmMinDivision;
            PrestrainKx = primitive.UsersPrestrain.Kx + primitive.AutoPrestrain.Kx;
            PrestrainKy = primitive.UsersPrestrain.Ky + primitive.AutoPrestrain.Ky;
            PrestrainEpsZ = primitive.UsersPrestrain.EpsZ + primitive.AutoPrestrain.EpsZ;
        }
    }
}
