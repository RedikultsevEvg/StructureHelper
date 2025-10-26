using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface IShapeTriangulationLogicOptions : ITriangulationLogicOptions, IHasCenter2D
    {
        /// <summary>
        /// Parameters of division
        /// </summary>
        IDivisionSize DivisionSize { get; }
    }
}
