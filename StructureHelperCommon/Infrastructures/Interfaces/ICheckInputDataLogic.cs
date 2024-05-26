using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface ICheckInputDataLogic : ICheckLogic
    {
        IInputData InputData { get; set; }
    }
}
