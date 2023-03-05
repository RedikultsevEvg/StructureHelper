using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <summary>
    /// Include parameters of triangulation for shapes
    /// </summary>
    public interface IHasDivisionSize
    {
        /// <summary>
        /// Maximum size of Ndm part
        /// </summary>
        double NdmMaxSize { get; set; }
        /// <summary>
        /// Mimimum division for sides of shape
        /// </summary>
        int NdmMinDivision { get; set; }
        /// <summary>
        /// Flag of removing ndm part which located inside shape
        /// </summary>
        bool ClearUnderlying { get; set; }
        /// <summary>
        /// Shows if point is located inside shape
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        bool IsPointInside(IPoint2D point);
    }
}
