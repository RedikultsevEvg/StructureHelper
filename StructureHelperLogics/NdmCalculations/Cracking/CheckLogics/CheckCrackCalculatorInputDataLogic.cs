using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic of checking of input data for crack calcultor 
    /// </summary>
    public class CheckCrackCalculatorInputDataLogic : ICheckInputDataLogic<ICrackCalculatorInputData>
    {
        private bool result;
        private ICheckEntityLogic<IHasPrimitives> checkPrimitiveCollectionLogic;

        public ICrackCalculatorInputData InputData {  get; set; }


        public string CheckResult { get; private set; }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckCrackCalculatorInputDataLogic(ICheckEntityLogic<IHasPrimitives> checkPrimitiveCollectionLogic)
        {
            this.checkPrimitiveCollectionLogic = checkPrimitiveCollectionLogic;
        }

        public CheckCrackCalculatorInputDataLogic() : this (new CheckPrimitiveCollectionLogic())
        {
            
        }

        public bool Check()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            result = true;
            CheckResult = string.Empty;
            CheckPrimitives();
            CheckActions();
            return result;
        }

        private void CheckPrimitives()
        {
            if (checkPrimitiveCollectionLogic is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": check primitive logic");
            }
            checkPrimitiveCollectionLogic.Entity = InputData;
            checkPrimitiveCollectionLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger();
            if (checkPrimitiveCollectionLogic.Check() == false)
            {
                result = false;
                CheckResult += checkPrimitiveCollectionLogic.CheckResult;
                TraceLogger?.AddMessage(checkPrimitiveCollectionLogic.CheckResult, TraceLogStatuses.Error);
            }
        }

        private void CheckActions()
        {
            if (InputData.ForceActions is null || (!InputData.ForceActions.Any()))
            {
                result = false;
                string message = "Calculator does not contain any actions\n";
                CheckResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                return;
            };
            var checkLogic = new CheckForceActionsLogic(TraceLogger)
            {
                Entity = InputData.ForceActions
            };
            if (checkLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(checkLogic.CheckResult);
        }
        private void TraceMessage(string errorString)
        {
            CheckResult += errorString + "\n";
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }


    }
}
