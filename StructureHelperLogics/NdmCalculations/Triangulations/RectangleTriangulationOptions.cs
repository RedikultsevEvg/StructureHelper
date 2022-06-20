using StructureHelperLogics.Data.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <inheritdoc />
    public class RectangleTriangulationOptions : IRectangleTriangulationOptions
    {
        /// <inheritdoc />
        public ICenter Center { get; }
        /// <inheritdoc />
        public IRectangle Rectangle { get; }
        /// <inheritdoc />
        public double NdmMaxSize { get; }
        /// <inheritdoc />
        public int NdmMinDivision { get; }

        public RectangleTriangulationOptions(ICenter center, IRectangle rectangle, double ndmMaxSize, int ndmMinDivision)
        {
            Center = center;
            Rectangle = rectangle;
            NdmMaxSize = ndmMaxSize;
            NdmMinDivision = ndmMinDivision;
        }
    }
}
