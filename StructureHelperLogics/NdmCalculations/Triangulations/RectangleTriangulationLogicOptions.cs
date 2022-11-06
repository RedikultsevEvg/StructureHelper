using System;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <inheritdoc />
    public class RectangleTriangulationLogicOptions : IRectangleTriangulationLogicOptions
    {
        /// <inheritdoc />
        public ICenter Center { get; }
        /// <inheritdoc />
        public IRectangle Rectangle { get; }
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

        public RectangleTriangulationLogicOptions(ICenter center, IRectangle rectangle, double ndmMaxSize, int ndmMinDivision)
        {
            Center = center;
            Rectangle = rectangle;
            NdmMaxSize = ndmMaxSize;
            NdmMinDivision = ndmMinDivision;
        }

        public RectangleTriangulationLogicOptions(INdmPrimitive primitive)
        {
            if (! (primitive.Shape is IRectangle)) { throw new StructureHelperException(ErrorStrings.ShapeIsNotCorrect); }
            Center = primitive.Center;
            Rectangle = primitive.Shape as IRectangle;
            NdmMaxSize = primitive.NdmMaxSize;
            NdmMinDivision = primitive.NdmMinDivision;
            PrestrainKx = primitive.PrestrainKx;
            PrestrainKy = primitive.PrestrainKy;
            PrestrainEpsZ = primitive.PrestrainEpsZ;
        }
    }
}
