using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculator : IForceCalculator
    {
        private IUpdateStrategy<IForceCalculator> updateStrategy;
        private ICheckInputDataLogic<IForceCalculatorInputData> checkInputDataLogic;
        private IForceCalculatorLogic forceCalculatorLogic;

        /// <inheritdoc/>
        public Guid Id { get; } = Guid.NewGuid();
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public IForceCalculatorInputData InputData { get; set; } = new ForceCalculatorInputData();
        /// <inheritdoc/>
        public Action<IResult> ActionToOutputResults { get; set; }
        /// <inheritdoc/>
        public IShiftTraceLogger? TraceLogger { get; set; }
        /// <inheritdoc/>
        public IResult Result { get; private set; }


        public ForceCalculator(
                ICheckInputDataLogic<IForceCalculatorInputData> checkInputDataLogic,
                IForceCalculatorLogic forceCalculatorLogic,
                IUpdateStrategy<IForceCalculator> updateStrategy
                )
        {
            this.checkInputDataLogic = checkInputDataLogic;
            this.forceCalculatorLogic = forceCalculatorLogic;
            this.updateStrategy = updateStrategy;
        }

        public ForceCalculator() :
            this(new CheckForceCalculatorInputData(),
                new ForceCalculatorLogic(),
                new ForceCalculatorUpdateStrategy())
        {
        }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
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
