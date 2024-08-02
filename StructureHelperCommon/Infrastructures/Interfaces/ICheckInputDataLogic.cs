using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Checks input data
    /// </summary>
    /// <typeparam name="TInputData">Class of input data</typeparam>
    public interface ICheckInputDataLogic<TInputData> : ICheckLogic where TInputData : IInputData
    {
        /// <summary>
        /// Class of input data
        /// </summary>
        TInputData InputData { get; set; }
    }
}
