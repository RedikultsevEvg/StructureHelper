using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IGetResultByInputDataLogic<T, V> : ILogic
        where T : IInputData
        where V : IResult
    {
        V GetResultByInputData(T inputData);
    }
}
