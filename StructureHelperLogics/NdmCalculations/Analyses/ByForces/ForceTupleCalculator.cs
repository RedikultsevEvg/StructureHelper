using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <inheritdoc/>
    public class ForceTupleCalculator : IForceTupleCalculator
    {
        IForceTupleCalculatorResult result;
        private ICheckInputDataLogic<IForceTupleInputData> checkInputDataLogic;
        private IForceTupleCalcLogic calcLogic;

        public IForceTupleInputData InputData { get; set; }
        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public Guid Id { get; }

        public ForceTupleCalculator(Guid id)
            : this()
        {
            Id = id;
        }

        public ForceTupleCalculator(ICheckInputDataLogic<IForceTupleInputData> checkInputDataLogic, IForceTupleCalcLogic calcLogic)
            : this(Guid.NewGuid(), checkInputDataLogic, calcLogic)
        {
        }

        public ForceTupleCalculator(
            Guid id,
            ICheckInputDataLogic<IForceTupleInputData> checkInputDataLogic,
            IForceTupleCalcLogic calcLogic)
        {
            Id = id;
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
            result = new ForceTupleCalculatorResult()
            {
                IsValid = true,
                Description = string.Empty,
                InputData = InputData,
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

        public ILogicCalculator GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            ForceTupleCalculator newItem = new(id);
            return newItem;
        }
    }
}
