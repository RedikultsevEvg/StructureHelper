using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears.Logics;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class CheckBeamShearCalculatorInputDataLogic : ICheckInputDataLogic<IBeamShearCalculatorInputData>
    {
        private bool result;
        private string checkResult;
        private ICheckEntityLogic<IBeamShearSection> checkSectionLogic;

        public string CheckResult => checkResult;
        public IBeamShearCalculatorInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public CheckBeamShearCalculatorInputDataLogic(IBeamShearCalculatorInputData inputData, IShiftTraceLogger? traceLogger)
        {
            InputData = inputData;
            TraceLogger = traceLogger;
        }

        public bool Check()
        {
            result = true;
            checkResult = string.Empty;
            if (InputData is null)
            {
                result = false;
                string errorString = ErrorStrings.ParameterIsNull + ": Input data";
                throw new StructureHelperException(errorString);
            }
            if (InputData.Actions is null || !InputData.Actions.Any())
            {
                result = false;
                string errorString = "Collection of actions does not contain any action";
                TraceMessage(errorString);
            }
            CheckSections();
            return result;
        }

        private void CheckSections()
        {
            if (InputData.Sections is null || !InputData.Sections.Any())
            {
                result = false;
                string errorString = "Collection of sections does not contain any section";
                TraceMessage(errorString);
            }
            else
            {
                checkSectionLogic ??= new CheckBeamShearSectionLogic(TraceLogger);
                foreach (var item in InputData.Sections)
                {
                    checkSectionLogic.Entity = item;
                    if (checkSectionLogic.Check() == false)
                    {
                        result = false;
                        checkResult += checkSectionLogic.CheckResult;
                    }
                }
            }
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString;
            TraceLogger?.AddMessage(errorString);
        }
    }
}
