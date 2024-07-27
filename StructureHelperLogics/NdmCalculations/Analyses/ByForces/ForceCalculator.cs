using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculator : ICalculator, IHasActionByResult
    {
        private IUpdateStrategy<ForceCalculator> updateStrategy = new ForceCalculatorUpdateStrategy();
        private ICheckInputDataLogic<ForceInputData> checkInputDataLogic;
        private IForceCalculatorLogic forceCalculatorLogic;


        public string Name { get; set; }
        public ForceInputData InputData {get;set;}
        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public IResult Result { get; private set; }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            checkInputDataLogic = new CheckForceCalculatorInputData(InputData);
            checkInputDataLogic.TraceLogger?.GetSimilarTraceLogger(50);
            if (checkInputDataLogic.Check() != true)
            {
                Result = new ForcesResults()
                {
                    IsValid = false,
                    Description = checkInputDataLogic.CheckResult
                };
                return;
            }
            if (ActionToOutputResults is not null)
            {
                forceCalculatorLogic.ActionToOutputResults = ActionToOutputResults;
            }
            forceCalculatorLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            Result = forceCalculatorLogic.GetForcesResults();
        }

        private void GetResult()
        {
            throw new NotImplementedException();
        }

        public ForceCalculator(ForceInputData inputData,
            ICheckInputDataLogic<ForceInputData> checkInputDataLogic,
            IForceCalculatorLogic forceCalculatorLogic,
            IUpdateStrategy<ForceCalculator> updateStrategy
            )
        {
            this.InputData = inputData;
            this.checkInputDataLogic = checkInputDataLogic;
            this.forceCalculatorLogic = forceCalculatorLogic;
            this.updateStrategy = updateStrategy;
        }

        public ForceCalculator(ForceInputData inputData) :
            this(inputData,
                new CheckForceCalculatorInputData(inputData),
                new ForceCalculatorLogic(inputData),
                new ForceCalculatorUpdateStrategy())
        {           
        }

        public ForceCalculator() : this(new ForceInputData())  { }

        public object Clone()
        {
            var newCalculator = new ForceCalculator(new ForceInputData());
            updateStrategy.Update(newCalculator, this);
            return newCalculator;
        }
    }
}
