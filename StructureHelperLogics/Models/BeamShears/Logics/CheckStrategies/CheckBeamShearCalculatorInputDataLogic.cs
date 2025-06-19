using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperLogics.Models.BeamShears.Logics;

namespace StructureHelperLogics.Models.BeamShears
{
    public class CheckBeamShearCalculatorInputDataLogic : ICheckInputDataLogic<IBeamShearCalculatorInputData>
    {
        private bool result;
        private string checkResult;
        private ICheckEntityLogic<IBeamShearAction> checkActionsLogic;
        private ICheckEntityLogic<IBeamShearSection> checkSectionLogic;
        private ICheckEntityLogic<IStirrup> checkStirrupLogic;

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
            CheckActions();
            CheckSections();
            CheckStirrups();
            return result;
        }

        private void CheckActions()
        {
            if (InputData.Actions is null || !InputData.Actions.Any())
            {
                result = false;
                string errorString = "\nCollection of actions does not contain any action";
                TraceMessage(errorString);
            }
            else
            {
                checkActionsLogic ??= new CheckBeamShearActionLogic(TraceLogger);
                foreach (var action in InputData.Actions)
                {
                    checkActionsLogic.Entity = action;
                    if (checkActionsLogic.Check() == false)
                    {
                        result = false;
                        checkResult += checkActionsLogic.CheckResult;
                    }
                }
            }
        }

        private void CheckStirrups()
        {
            if (InputData.Stirrups is null)
            {
                result = false;
                TraceMessage("\nCollection of stirrups is null");
            }
            else
            {
                checkStirrupLogic ??= new CheckStirrupsLogic(TraceLogger);
                foreach (var stirrup in InputData.Stirrups)
                {
                    checkStirrupLogic.Entity = stirrup;
                    if (checkStirrupLogic.Check() == false)
                    {
                        result = false;
                        checkResult += checkStirrupLogic.CheckResult;
                    }
                }
            }
        }

        private void CheckSections()
        {
            if (InputData.Sections is null || !InputData.Sections.Any())
            {
                result = false;
                TraceMessage("\nCollection of sections does not contain any section");
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
