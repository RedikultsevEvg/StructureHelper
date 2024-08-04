using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    internal class CheckForceCalculatorInputData : ICheckInputDataLogic<IForceInputData>
    {
        private bool result;
        private string checkResult;
        private ICheckEntityLogic<IAccuracy> checkAccuracyLogic;

        public IForceInputData InputData { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckForceCalculatorInputData(ICheckEntityLogic<IAccuracy> checkAccuracyLogic)
        {
            this.checkAccuracyLogic = checkAccuracyLogic;
        }

        public CheckForceCalculatorInputData() : this(new CheckAccuracyLogic())
        {
            
        }

        public bool Check()
        {
            result = true;
            checkResult = string.Empty;
            if (InputData is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": input data";
                TraceMessage(errorString);
                throw new StructureHelperException(errorString);
            }
            if (InputData.Primitives is null || !InputData.Primitives.Any())
            {
                TraceMessage("Calculator does not contain any primitives");
                result = false;
            }
            if (InputData.ForceActions is null || !InputData.ForceActions.Any())
            {
                TraceMessage("Calculator does not contain any forces");
                result = false;
            }
            if (InputData.LimitStatesList is null || !InputData.LimitStatesList.Any())
            {
                TraceMessage("Calculator does not contain any limit states");
                result = false;
            }
            if (InputData.CalcTermsList is null || !InputData.CalcTermsList.Any())
            {
                TraceMessage("Calculator does not contain any calc term");
                result = false;
            }
            CheckAccuracy();
            return result;
        }

        private void CheckAccuracy()
        {
            checkAccuracyLogic.Entity = InputData.Accuracy;
            checkAccuracyLogic.TraceLogger = TraceLogger;
            if (checkAccuracyLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(checkAccuracyLogic.CheckResult);
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString + "\n";
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
