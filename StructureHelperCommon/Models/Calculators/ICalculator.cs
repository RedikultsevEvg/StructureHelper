using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager;

namespace StructureHelperCommon.Models.Calculators
{
    public interface ICalculator : ICloneable
    {
        string Name { get; set; }
        /// <summary>
        /// Method for calculating
        /// </summary>
        void Run();
        /// <summary>
        /// Result of Calculations
        /// </summary>
        IResult Result { get; }
    }
}
