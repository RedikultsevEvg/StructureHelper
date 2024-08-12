using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CheckRebarCrackCalculatorInputDataLogic : ICheckInputDataLogic<IRebarCrackCalculatorInputData>
    {
        private string checkResult;
        private bool result;

        public IRebarCrackCalculatorInputData InputData { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool Check()
        {
            result = true;
            checkResult = string.Empty;
            if (InputData is null)
            {
                TraceMessage(ErrorStrings.ParameterIsNull + ": input data");
                result = false;
            }
            if (InputData.LongRebarData is null)
            {
                TraceMessage(ErrorStrings.ParameterIsNull + ": long term input data for rebar");
                result = false;
            }
            if (InputData.ShortRebarData is null)
            {
                TraceMessage(ErrorStrings.ParameterIsNull + ": short term input data for rebar");
                result = false;
            }
            if (InputData.RebarPrimitive is null)
            {
                TraceMessage(ErrorStrings.ParameterIsNull + ": rebar primitive");
                result = false;
            }
            if (InputData.UserCrackInputData is null)
            {
                TraceMessage(ErrorStrings.ParameterIsNull + ": user crack input data");
                result = false;
            }
            return result;
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString + "\n";
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
