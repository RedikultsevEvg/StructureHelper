using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Sections
{
    /// <summary>
    /// Logic for calculating of some value
    /// </summary>
    public interface IProcessorLogic<T> : ILogic
    {
        /// <summary>
        /// Returns new value
        /// </summary>
        /// <returns></returns>
        T GetValue();
    }
}
