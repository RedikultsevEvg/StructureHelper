using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class CheckSectionLogicInputDataLogic : ICheckInputDataLogic<IBeamShearSectionLogicInputData>
    {
        private bool result;
        private string checkResult;
        public CheckSectionLogicInputDataLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IBeamShearSectionLogicInputData InputData { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

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
            if (InputData.ForceTuple is null)
            {
                result = false;
                TraceMessage("Force tuple is null");
            }
            if (InputData.ForceTuple.Qy < 0)
            {
                result = false;
                TraceMessage($"Shear force Qy = {InputData.ForceTuple.Qy} must be positive");
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
