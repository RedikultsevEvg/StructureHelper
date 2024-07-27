using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CheckTupleCalculatorInputData : ICheckInputDataLogic<TupleCrackInputData>
    {
        private string checkResult;
        private bool result;

        public TupleCrackInputData InputData { get; set; }


        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckTupleCalculatorInputData(TupleCrackInputData inputData)
        {
            InputData = inputData;
        }

        public bool Check()
        {
            result = true;
            return result;
        }
    }
}
