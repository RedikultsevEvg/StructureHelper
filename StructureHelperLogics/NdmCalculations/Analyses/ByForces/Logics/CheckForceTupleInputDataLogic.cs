using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class CheckForceTupleInputDataLogic : ICheckInputDataLogic<IForceTupleInputData>
    {
        private bool result;
        private string checkResult;

        public IForceTupleInputData InputData { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckForceTupleInputDataLogic(IForceTupleInputData inputData, IShiftTraceLogger traceLogger)
        {
            InputData = inputData;
            TraceLogger = traceLogger;
        }

        public CheckForceTupleInputDataLogic()
        {
            
        }

        public bool Check()
        {
            result = true;
            checkResult = string.Empty;
            if (InputData is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": Input data";
                TraceLogger?.AddMessage(errorString);
                throw new StructureHelperException(errorString);
            }
            if (InputData.NdmCollection is null || ! InputData.NdmCollection.Any())
            {
                result = false;
                string errorString = "\nNdm collection is null or empty";
                TraceMessage(errorString);
            }
            if (InputData.ForceTuple is null)
            {
                result = false;
                string errorString = "\nForce tuple is null";
                TraceMessage(errorString);
            }
            if (InputData.Accuracy is null)
            {
                result = false;
                string errorString = "\nAccuracy requirements is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                if (InputData.Accuracy.IterationAccuracy <= 0)
                {
                    result = false;
                    TraceMessage($"\nValue of accuracy {InputData.Accuracy.IterationAccuracy} must be grater than zero");
                }
                if (InputData.Accuracy.MaxIterationCount <= 0)
                {
                    result = false;
                    TraceMessage($"\nMax number of iteration {InputData.Accuracy.MaxIterationCount} must be grater than zero");
                }
            }
            return result;
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString;
            TraceLogger?.AddMessage(errorString);
        }
    }
}
