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

        private CrackResult result;
        private IGetTupleInputDatasLogic datasLogic;
        private CrackCalculatorUpdateStrategy updateStrategy = new();

        public string Name { get; set; }
        public CrackInputData InputData { get; set; }
        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public CrackCalculator(CrackInputData inputData)
        {
            InputData = inputData;
        }

        public object Clone()
        {
            var newItem = new CrackCalculator(new CrackInputData());
            updateStrategy.Update(newItem, this);
            return newItem;
        }

        public void Run()
        {
            PrepareNewResult();
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

        private void ProcessCalculations()
        {
            datasLogic = new GetTupleInputDatasLogic(InputData.Primitives, InputData.ForceActions, InputData.UserCrackInputData)
            {
                LimitState = limitState,
                LongTerm = longTerm,
                ShortTerm = shortTerm,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
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
