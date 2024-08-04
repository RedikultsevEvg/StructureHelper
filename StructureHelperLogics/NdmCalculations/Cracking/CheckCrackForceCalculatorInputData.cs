using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CheckCrackForceCalculatorInputData : ICheckInputDataLogic<ICrackForceCalculatorInputData>
    {
        private string checkResult;
        private bool result;
        public ICrackForceCalculatorInputData InputData { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool Check()
        {
            result = true;
            if (InputData is null)
            {
                TraceMessage("InputData is null");
                return false;
            }
            if (InputData.CheckedNdmCollection is null || ! InputData.CheckedNdmCollection.Any())
            {
                result = false;
                TraceMessage("Checked ndm collection is null or empty");
            }
            if (InputData.SectionNdmCollection is null || !InputData.SectionNdmCollection.Any())
            {
                result = false;
                TraceMessage("Section ndm collection is null or empty");
            }
            if (InputData.StartTuple is null)
            {
                result = false;
                TraceMessage("Start force tuple is null");
            }
            if (InputData.EndTuple is null)
            {
                result = false;
                TraceMessage("End force tuple is null");
            }
            if (InputData.StartTuple == InputData.EndTuple)
            {
                result = false;
                TraceMessage("Start tuple is equal to end tuple");
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
