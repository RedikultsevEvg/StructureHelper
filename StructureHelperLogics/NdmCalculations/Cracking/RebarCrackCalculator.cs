using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarCrackCalculator : ICalculator
    {
        ICrackWidthLogic crackWidthLogic = new CrackWidthLogicSP63();
        RebarCrackResult result;
        public string Name { get; set; }
        public RebarCrackCalculatorInputData InputData { get; set; }
        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            result = new()
            {
                IsValid = true
            };
            TraceLogger?.AddMessage($"Rebar primitive {InputData.RebarPrimitive.Name}");

            //double acrc1 = GetCrackWidth()


            crackWidthLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            try
            {
                var dataAcrc1 = GetCrackWidthInputData(InputData.LongRebarData, CalcTerms.LongTerm);
                var dataAcrc2 = GetCrackWidthInputData(InputData.LongRebarData, CalcTerms.ShortTerm);
                var dataAcrc3 = GetCrackWidthInputData(InputData.ShortRebarData, CalcTerms.ShortTerm);

                crackWidthLogic.InputData = dataAcrc1;       
                var acrc1 = crackWidthLogic.GetCrackWidth();

                var longRebarResult = new CrackWidthTupleResult()
                {
                    CrackWidth = acrc1,
                };
                result.LongTermResult = longRebarResult;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += "\n" + ex;
            }
            result.RebarPrimitive = InputData.RebarPrimitive;
        }

        private ICrackWidthLogicInputData GetCrackWidthInputData(RebarCrackInputData inputData, CalcTerms calcTerm)
        {
            var factoryInputData = new CrackWidthLogicInputDataFactory()
            {
                CalcTerm = calcTerm,
                InputData = inputData,
                RebarPrimitive = InputData.RebarPrimitive,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            var crackWidthInputData = factoryInputData.GetCrackWidthLogicInputData();
            return crackWidthInputData;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
