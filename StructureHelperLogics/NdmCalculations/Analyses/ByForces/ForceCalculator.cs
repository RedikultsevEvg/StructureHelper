using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculator : ICalculator, IHasActionByResult
    {
        private IUpdateStrategy<ForceCalculator> updateStrategy;
        private ICheckInputDataLogic<IForceInputData> checkInputDataLogic;
        private IForceCalculatorLogic forceCalculatorLogic;


        public string Name { get; set; }
        public ForceInputData InputData {get;set;}
        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public IResult Result { get; private set; }

        public ForceCalculator(ICheckInputDataLogic<IForceInputData> checkInputDataLogic,
                IForceCalculatorLogic forceCalculatorLogic,
                IUpdateStrategy<ForceCalculator> updateStrategy
                )
        {
            this.checkInputDataLogic = checkInputDataLogic;
            this.forceCalculatorLogic = forceCalculatorLogic;
            this.updateStrategy = updateStrategy;

            InputData = new ForceInputData();
        }

        public ForceCalculator() :
            this(new CheckForceCalculatorInputData(),
                new ForceCalculatorLogic(),
                new ForceCalculatorUpdateStrategy())
        {
        }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            checkInputDataLogic.InputData = InputData;
            checkInputDataLogic.TraceLogger = TraceLogger;
            if (checkInputDataLogic.Check() != true)
            {
                Result = new ForcesResults()
                {
                    IsValid = false,
                    Description = checkInputDataLogic.CheckResult
                };
                return;
            }
            forceCalculatorLogic.InputData = InputData;
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

        public object Clone()
        {
            var newCalculator = new ForceCalculator();
            updateStrategy.Update(newCalculator, this);
            return newCalculator;
        }
    }
}
