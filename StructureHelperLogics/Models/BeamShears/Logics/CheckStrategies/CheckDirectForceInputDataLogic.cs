using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;

namespace StructureHelperLogics.Models.BeamShears
{
    public class CheckDirectForceInputDataLogic : ICheckInputDataLogic<IDirectShearForceLogicInputData>
    {
        private string checkResult;
        private bool result;
        private ICheckEntityLogic<IInclinedSection> checkInclinedSectionLogic;
        private ICheckEntityLogic<IBeamShearAction> checkBeamShearActionLogic;

        public string CheckResult => checkResult;

        public IDirectShearForceLogicInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckDirectForceInputDataLogic(IDirectShearForceLogicInputData inputData, IShiftTraceLogger? traceLogger)
        {
            InputData = inputData;
            TraceLogger = traceLogger;
        }


        public bool Check()
        {
            InitializeStrategies();
            result = true;
            if (InputData is null)
            {
                result = false;
                string errorString = "\nInput data is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                CheckBeamShearAction();
                CheckInclinedSection();
            }
            return result;
        }

        private void InitializeStrategies()
        {
            checkInclinedSectionLogic ??= new CheckInclinedSectionLogic(InputData.InclinedSection, TraceLogger);
            checkBeamShearActionLogic ??= new CheckBeamShearActionLogic(TraceLogger);
        }

        private void CheckBeamShearAction()
        {
            if (InputData.BeamShearAction is null)
            {
                result = false;
                TraceMessage($"\nBeam shear action is not assigned");
            }
            else
            {
                checkBeamShearActionLogic.Entity = InputData.BeamShearAction;
                if (checkBeamShearActionLogic.Check() == false)
                {
                    result = false;
                    checkResult += checkBeamShearActionLogic.CheckResult;
                }
            }
        }

        private void CheckInclinedSection()
        {
            if (InputData.InclinedSection is null)
            {
                result = false;
                TraceMessage($"\nInclined section is not assigned");
            }
            else
            {
                if (checkInclinedSectionLogic.Check() == false)
                {
                    result = false;
                    checkResult += checkInclinedSectionLogic.CheckResult;
                }
            }
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString;
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
