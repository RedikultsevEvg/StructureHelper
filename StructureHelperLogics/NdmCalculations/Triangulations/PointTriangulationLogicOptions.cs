using StructureHelperLogics.Data.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

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

        public double Area { get; }

        public PointTriangulationLogicOptions(ICenter center, double area)
        {
            Center = center;
            Area = area;
        }
    }
}
