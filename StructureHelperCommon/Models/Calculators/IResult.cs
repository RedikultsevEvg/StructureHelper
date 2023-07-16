using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public interface IResult
    {
        /// <summary>
        /// True if result of calculation is valid
        /// </summary>
        bool IsValid { get; set; }
        string Description { get; set; }
    }
}
