using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackCalculator : ICalculator
    {
        const LimitStates limitState = LimitStates.SLS;
        const CalcTerms longTerm = CalcTerms.LongTerm;
        const CalcTerms shortTerm = CalcTerms.ShortTerm;
        private const double maxSizeOfCrossSection = 1d;
        private CrackResult result;
        private IGetTupleInputDatasLogic datasLogic;
        private CrackCalculatorUpdateStrategy updateStrategy = new();
        private ICheckInputDataLogic<CrackInputData> checkInputDataLogic;

        public string Name { get; set; }
        public CrackInputData InputData { get; set; }
        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public CrackCalculator(CrackInputData inputData, ICheckInputDataLogic<CrackInputData> checkInputDataLogic)
        {
            InputData = inputData;
            this.checkInputDataLogic = checkInputDataLogic;
            Name = string.Empty;
        }

        public CrackCalculator(CrackInputData inputData)
            : this(inputData,
                  new CheckCrackCalculatorInputDataLogic()
                  { InputData = inputData}
                  ) { }

        public object Clone()
        {
            CrackInputData crackInputData = new CrackInputData();
            var checkDataLogic = new CheckCrackCalculatorInputDataLogic()
            {
                InputData = InputData
            };
            var newItem = new CrackCalculator(crackInputData, checkDataLogic);
            updateStrategy.Update(newItem, this);
            return newItem;
        }

        public void Run()
        {
            PrepareNewResult();
            CheckInputData();
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            try
            {
                ProcessCalculations();
                TraceLogger?.AddMessage(LoggerStrings.CalculationHasDone);
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex;
                TraceLogger?.AddMessage(LoggerStrings.CalculationError + ex, TraceLogStatuses.Error);
            }
        }

        private void CheckInputData()
        {
            checkInputDataLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            if (checkInputDataLogic.Check() == false)
            {
                result.IsValid = false;
                result.Description += checkInputDataLogic.CheckResult;
            }
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
