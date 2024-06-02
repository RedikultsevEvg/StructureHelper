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
    public class CheckTupleCalculatorInputData : ICheckInputDataLogic
    {
        private string checkResult;
        private TupleCrackInputData inputData;
        private bool result;

        public IInputData InputData
        {
            get => inputData; set
            {
                if (value is TupleCrackInputData data)
                {
                    inputData = data;
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect);
                }
            }
        }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool Check()
        {
            result = true;
            return result;
        }
    }
}
