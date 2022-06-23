using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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

        public RectangleTriangulationLogicOptions(ICenter center, IRectangle rectangle, double ndmMaxSize, int ndmMinDivision)
        {
            Center = center;
            Rectangle = rectangle;
            NdmMaxSize = ndmMaxSize;
            NdmMinDivision = ndmMinDivision;
        }

        public RectangleTriangulationLogicOptions(INdmPrimitive primitive)
        {
            if (! (primitive.Shape is IRectangle)) { throw new Exception("Shape type is not valid"); }
            Center = primitive.Center;
            Rectangle = primitive.Shape as IRectangle;
            NdmMaxSize = primitive.NdmMaxSize;
            NdmMinDivision = primitive.NdmMinDivision;
        }
    }
}
