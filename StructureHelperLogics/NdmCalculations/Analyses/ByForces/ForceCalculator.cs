using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculator : IForceCalculator
    {
        private ICheckInputDataLogic<IForceCalculatorInputData>? checkInputDataLogic;
        private IForceCalculatorLogic? forceCalculatorLogic;
        private IUpdateStrategy<IForceCalculator>? updateStrategy;
        private ICheckInputDataLogic<IForceCalculatorInputData> CheckInputDataLogic => checkInputDataLogic ??= new CheckForceCalculatorInputData();
        private IForceCalculatorLogic ForceCalculatorLogic => forceCalculatorLogic ??= new ForceCalculatorLogic();
        private IUpdateStrategy<IForceCalculator> UpdateStrategy => updateStrategy ??= new ForceCalculatorUpdateStrategy();

        /// <inheritdoc/>
        public Guid Id { get; } = Guid.NewGuid();
        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;
        /// <inheritdoc/>
        public IForceCalculatorInputData InputData { get; set; } = new ForceCalculatorInputData();
        /// <inheritdoc/>
        public Action<IResult> ActionToOutputResults { get; set; }
        /// <inheritdoc/>
        public IShiftTraceLogger? TraceLogger { get; set; }
        /// <inheritdoc/>
        public IResult Result { get; private set; }
        public bool ShowTraceData { get; set; }

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

        public ForceCalculator(Guid id)
        {
            Id = id;
        }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            CheckInputDataLogic.InputData = InputData;
            CheckInputDataLogic.TraceLogger = TraceLogger;
            if (CheckInputDataLogic.Check() != true)
            {
                Result = new ForceCalculatorResult()
                {
                    IsValid = false,
                    Description = CheckInputDataLogic.CheckResult
                };
                return;
            }
            ForceCalculatorLogic.InputData = InputData;
            if (ActionToOutputResults is not null)
            {
                ForceCalculatorLogic.ActionToOutputResults = ActionToOutputResults;
            }
            ForceCalculatorLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            Result = ForceCalculatorLogic.GetForcesResults();
        }

        public object Clone()
        {
            var newCalculator = new ForceCalculator(Guid.NewGuid());
            UpdateStrategy.Update(newCalculator, this);
            return newCalculator;
        }
    }
}
