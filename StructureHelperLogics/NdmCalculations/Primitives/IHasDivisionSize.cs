using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface IHasDivisionSize
    {
        ///<inheritdoc/>
        IDivisionSize DivisionSize { get; }
        /// <summary>
        /// Shows if point is located inside shape
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        bool IsPointInside(IPoint2D point);
    }
}
