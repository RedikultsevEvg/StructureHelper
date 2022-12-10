using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public interface INdmResult
    {
        /// <summary>
        /// True if result of calculation is valid
        /// </summary>
        bool IsValid { get; }
    }
}
