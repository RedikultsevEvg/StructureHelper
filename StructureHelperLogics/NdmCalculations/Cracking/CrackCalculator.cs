using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <inheritdoc/>
    public class CrackCalculator : ICrackCalculator
    {
        const LimitStates limitState = LimitStates.SLS;
        const CalcTerms longTerm = CalcTerms.LongTerm;
        const CalcTerms shortTerm = CalcTerms.ShortTerm;
        private const double maxSizeOfCrossSection = 1d;
        private CrackResult result;

        private IGetTupleInputDatasLogic datasLogic;

        private ICheckEntityLogic<ICrackCalculatorInputData> checkInputDataLogic;
        private IUpdateStrategy<ICrackCalculator> updateStrategy;
        private ICheckEntityLogic<ICrackCalculatorInputData> CheckInputDataLogic => checkInputDataLogic ??= new CheckCrackCalculatorInputDataLogic();
        private IUpdateStrategy<ICrackCalculator> UpdateStrategy => updateStrategy ??= new CrackCalculatorUpdateStrategy();

        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public ICrackCalculatorInputData InputData { get; set; }
        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public bool ShowTraceData { get; set; }

        public CrackCalculator(ICheckEntityLogic<ICrackCalculatorInputData> checkInputDataLogic,
            IUpdateStrategy<ICrackCalculator> updateStrategy,
            IShiftTraceLogger traceLogger
            )
        {
            this.checkInputDataLogic = checkInputDataLogic;
            this.updateStrategy = updateStrategy;
            this.TraceLogger = traceLogger;
        }

        public CrackCalculator(Guid id, IShiftTraceLogger traceLogger)
        {
            Id = id;
            TraceLogger = traceLogger;
        }

        public object Clone()
        {
            CrackCalculatorInputData crackInputData = new CrackCalculatorInputData();
            var checkDataLogic = new CheckCrackCalculatorInputDataLogic()
            {
                Entity = InputData
            };
            var newItem = new CrackCalculator(checkDataLogic, new CrackCalculatorUpdateStrategy(), new ShiftTraceLogger());
            newItem.InputData = crackInputData;
            UpdateStrategy.Update(newItem, this);
            return newItem;
        }

        public void Run()
        {
            PrepareNewResult();
            if (CheckInputData() == false)
            {
                return;
            }
            TraceInputData();
            try
            {
                ProcessCalculations();
                TraceResult();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex;
                TraceLogger?.AddMessage(LoggerStrings.CalculationError + ex, TraceLogStatuses.Error);
            }
        }

        private void TraceResult()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculationHasDone);
            ITraceEntityLogic traceLogic = new TraceCrackResultLogic()
            {
                Collection = result.TupleResults
            };
            traceLogic.AddEntriesToTraceLogger(TraceLogger);
        }

        private void TraceInputData()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            ITraceEntityLogic traceLogic = new TracePrimitiveFactory()
            {
                Collection = InputData.Primitives
            };
            traceLogic.AddEntriesToTraceLogger(TraceLogger);
            traceLogic = new TraceMaterialsFactory()
            {
                Collection = InputData.Primitives.Select(x => x.NdmElement.HeadMaterial).Distinct()
            };
            traceLogic.AddEntriesToTraceLogger(TraceLogger);
        }

        private bool CheckInputData()
        {
            CheckInputDataLogic.Entity = InputData;
            CheckInputDataLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            if (CheckInputDataLogic.Check() == false)
            {
                result.IsValid = false;
                result.Description += CheckInputDataLogic.CheckResult;
                return false;
            }
            return true;
        }

        private void ProcessCalculations()
        {
            datasLogic = new GetTupleInputDatasLogic(InputData.Primitives, InputData.ForceActions, InputData.UserCrackInputData)
            {
                LimitState = limitState,
                LongTerm = longTerm,
                ShortTerm = shortTerm,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            var dx = InputData.Primitives.Max(x => x.GetValuePoints().Max(y => y.Point.X)) - InputData.Primitives.Min(x => x.GetValuePoints().Min(y => y.Point.X));
            var dy = InputData.Primitives.Max(x => x.GetValuePoints().Max(y => y.Point.Y)) - InputData.Primitives.Min(x => x.GetValuePoints().Min(y => y.Point.Y));
            if (dx > maxSizeOfCrossSection || dy > maxSizeOfCrossSection)
            {
                string message = $"At least one of size of cross-section is greater than ultimate size MaxSize = {maxSizeOfCrossSection}(m)";
                result.Description += "Warning! " + message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Warning);
            }
            var datas = datasLogic.GetTupleInputDatas();
            foreach (var data in datas)
            {
                var calculator = new TupleCrackCalculator()
                {
                    InputData = data,
                    TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
                };
                calculator.Run();
                var calcResult = calculator.Result as TupleCrackResult;
                result.TupleResults.Add(calcResult);
            }
        }

        private void PrepareNewResult()
        {
            result = new CrackResult
            {
                IsValid = true,
                Description = string.Empty
            };
        }
    }
}
