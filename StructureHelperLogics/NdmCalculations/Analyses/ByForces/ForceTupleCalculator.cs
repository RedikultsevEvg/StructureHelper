using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <inheritdoc/>
    public class ForceTupleCalculator : IForceTupleCalculator
    {
        IForcesTupleResult result;
        private ICheckInputDataLogic<IForceTupleInputData> checkInputDataLogic;
        private IForceTupleCalcLogic calcLogic;

        public IForceTupleInputData InputData { get; set; }
        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public Guid Id => throw new NotImplementedException();

        public ForceTupleCalculator(ICheckInputDataLogic<IForceTupleInputData> checkInputDataLogic, IForceTupleCalcLogic calcLogic)
        {
            this.checkInputDataLogic = checkInputDataLogic;
            this.calcLogic = calcLogic;
        }

        public ForceTupleCalculator() : this(new CheckForceTupleInputDataLogic(), new ForceTupleCalcLogic())
        {
            
        }

        public void Run()
        {
            PrepareNewResult();
            if (CheckInputData() == false)
            {
                return;
            }
            CalculateResult();
        }
        public object Clone()
        {
            var newItem = new ForceTupleCalculator();
            return newItem;
        }

        private void CalculateResult()
        {
            calcLogic.InputData = InputData;
            calcLogic.ActionToOutputResults = ActionToOutputResults;
            calcLogic.TraceLogger = TraceLogger;
            calcLogic.Calculate();
            result = calcLogic.Result;
        }

        private void PrepareNewResult()
        {
            result = new ForcesTupleResult()
            {
                IsValid = true,
                Description = string.Empty,
            };
        }

        private bool CheckInputData()
        {
            checkInputDataLogic.InputData = InputData;
            checkInputDataLogic.TraceLogger = TraceLogger;
            if (checkInputDataLogic.Check() == false)
            {
                result.IsValid = false;
                result.Description += checkInputDataLogic.CheckResult;
                return false;
            }
            return true;
        }

        

    }
}
