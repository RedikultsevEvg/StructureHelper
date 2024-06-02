using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface IShapeTriangulationLogicOptions : ITriangulationLogicOptions
    {
        /// <summary>
        /// Center of shape
        /// </summary>
        IPoint2D Center { get; }
        /// <summary>
        /// Maximum size (width or height) of ndm part after triangulation
        /// </summary>
        double NdmMaxSize { get; }
        /// <summary>
        /// Minimum quantity of division of side of rectangle after triangulation
        /// </summary>
        int NdmMinDivision { get; }
    }
}
