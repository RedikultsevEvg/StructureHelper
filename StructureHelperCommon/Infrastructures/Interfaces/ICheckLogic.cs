using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Logic for checking entities
    /// </summary>
    public interface ICheckLogic : ILogic
    {
        /// <summary>
        /// Text result of checking
        /// </summary>
        string CheckResult { get; }
        /// <summary>
        /// Start checking process
        /// </summary>
        /// <returns></returns>
        bool Check();
    }
}
