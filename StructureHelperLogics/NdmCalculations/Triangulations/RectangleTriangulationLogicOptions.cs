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
        public ICenter Center { get; }
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

        public RectangleTriangulationLogicOptions(ICenter center, IRectangleShape rectangle, double ndmMaxSize, int ndmMinDivision)
        {
            Center = center;
            Rectangle = rectangle;
            NdmMaxSize = ndmMaxSize;
            NdmMinDivision = ndmMinDivision;
        }

        public RectangleTriangulationLogicOptions(INdmPrimitive primitive)
        {
            if (! (primitive.Shape is IRectangleShape)) { throw new StructureHelperException(ErrorStrings.ShapeIsNotCorrect); }
            Center = primitive.Center;
            Rectangle = primitive.Shape as IRectangleShape;
            if (primitive is IHasDivisionSize)
            {
                NdmMaxSize = (primitive as IHasDivisionSize).NdmMaxSize;
                NdmMinDivision = (primitive as IHasDivisionSize).NdmMinDivision;
            }
            PrestrainKx = primitive.PrestrainKx;
            PrestrainKy = primitive.PrestrainKy;
            PrestrainEpsZ = primitive.PrestrainEpsZ;
        }
    }
}
