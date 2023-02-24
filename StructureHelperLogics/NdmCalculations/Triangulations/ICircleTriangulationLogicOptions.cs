using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    internal interface ICircleTriangulationLogicOptions : IShapeTriangulationLogicOptions
    {
        /// <summary>
        /// Shape
        /// </summary>
        ICircleShape Circle { get; }

    }
}
