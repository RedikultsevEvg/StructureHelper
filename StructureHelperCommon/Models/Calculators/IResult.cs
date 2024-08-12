using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    /// <summary>
    /// Base interface of result of calculation
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// True if result of calculation is valid
        /// </summary>
        bool IsValid { get; set; }
        /// <summary>
        /// Description of result of calculation
        /// </summary>
        string? Description { get; set; }
    }
}
