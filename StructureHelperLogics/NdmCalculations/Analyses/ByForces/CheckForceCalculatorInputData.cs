using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    internal class CheckForceCalculatorInputData : ICheckInputDataLogic<ForceInputData>
    {
        
        public ForceInputData InputData { get; set; }

        public string CheckResult { get; private set; }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckForceCalculatorInputData(ForceInputData inputData)
        {
            InputData = inputData;
            CheckResult = string.Empty;
        }

        public bool Check()
        {
            bool result = true;
            CheckResult = string.Empty;
            if (!InputData.Primitives.Any())
            {
                CheckResult += "Calculator does not contain any primitives \n";
                result = false;
            }
            if (!InputData.ForceActions.Any())
            {
                CheckResult += "Calculator does not contain any forces \n";
                result = false;
            }
            if (!InputData.LimitStatesList.Any())
            {
                CheckResult += "Calculator does not contain any limit states \n";
                result = false;
            }
            if (!InputData.CalcTermsList.Any())
            {
                CheckResult += "Calculator does not contain any calc term \n";
                result = false;
            }
            return result;
        }
    }
}
