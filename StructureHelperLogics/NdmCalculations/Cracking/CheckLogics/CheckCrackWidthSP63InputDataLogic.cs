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
    public class CheckCrackWidthSP63InputDataLogic : ICheckInputDataLogic<CrackWidthLogicInputDataSP63>
    {
        private string checkResult;
        private bool result;

        public CrackWidthLogicInputDataSP63 InputData { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckCrackWidthSP63InputDataLogic(IShiftTraceLogger? traceLogger)
        {
            this.TraceLogger = traceLogger;
        }

        public CheckCrackWidthSP63InputDataLogic() :this(null)
        {
            
        }

        public bool Check()
        {
            result = true;
            checkResult = string.Empty;
            string errorString;
            if (InputData.LengthBetweenCracks <= 0d)
            {
                errorString = ErrorStrings.DataIsInCorrect + $": length between cracks Lcrc={InputData.LengthBetweenCracks} must be greater than zero";
                TraceMessage(errorString);
                result = false;
            }
            if (InputData.TermFactor <= 0d)
            {
                errorString = ErrorStrings.DataIsInCorrect + $": Term factor fi1 {InputData.TermFactor} must be greater than zero";
                TraceMessage(errorString);
                result = false;
            }
            if (InputData.BondFactor <= 0d)
            {
                errorString = ErrorStrings.DataIsInCorrect + $": Bond factor fi2 {InputData.BondFactor} must be greater than zero";
                TraceMessage(errorString);
                result = false;
            }
            if (InputData.StressStateFactor <= 0d)
            {
                errorString = ErrorStrings.DataIsInCorrect + $": Stress factor fi3 factor {InputData.StressStateFactor} must be greater than zero";
                TraceMessage(errorString);
                result = false;
            }
            if (InputData.PsiSFactor <= 0d)
            {
                errorString = ErrorStrings.DataIsInCorrect + $": PsiS factor {InputData.PsiSFactor} must be greater than zero";
                TraceMessage(errorString);
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
