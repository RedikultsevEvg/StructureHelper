using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Calculators
{
    public interface ICalculator : ILogicCalculator, ISelfCloneStrategy<ICalculator>
    {      
        string Name { get; set; }
        /// <summary>
        /// Flag of exibiting of trace data
        /// </summary>
        bool ShowTraceData { get; set; }

    }
}
