using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarCrackCalculator : ICalculator
    {
        private ICrackSofteningLogic crackSofteningLogic;
        private ICrackWidthLogic crackWidthLogic = new CrackWidthLogicSP63();
        private RebarCrackResult result;
        private RebarStressResult rebarStressResult;

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
                GetSofteningLogic(InputData.LongRebarData);
                rebarStressResult = GetRebarStressResult(InputData.LongRebarData);
                var dataAcrc1 = GetCrackWidthInputData(InputData.LongRebarData, CalcTerms.LongTerm);
                var dataAcrc2 = GetCrackWidthInputData(InputData.LongRebarData, CalcTerms.ShortTerm);
                crackWidthLogic.InputData = dataAcrc1;
                var acrc1 = crackWidthLogic.GetCrackWidth();
                var longRebarResult = new CrackWidthTupleResult()
                {
                    CrackWidth = acrc1,
                    UltimateCrackWidth = InputData.UserCrackInputData.UltimateLongCrackWidth,
                    RebarStrain = rebarStressResult.RebarStrain,
                    ConcreteStrain = rebarStressResult.ConcreteStrain
                };

                GetSofteningLogic(InputData.ShortRebarData);
                rebarStressResult = GetRebarStressResult(InputData.ShortRebarData);
                var dataAcrc3 = GetCrackWidthInputData(InputData.ShortRebarData, CalcTerms.ShortTerm);

                crackWidthLogic.InputData = dataAcrc2;
                var acrc2 = crackWidthLogic.GetCrackWidth();
                crackWidthLogic.InputData = dataAcrc3;
                var acrc3 = crackWidthLogic.GetCrackWidth();

                double acrcShort = acrc1 - acrc2 + acrc3;
                TraceLogger?.AddMessage($"Long crack width acrc = acrc,1 = {acrc1}(m)");
                TraceLogger?.AddMessage($"Short crack width acrc = acrc,1 - acrc,2 + acrc,3 = {acrc1} - {acrc2} + {acrc3} = {acrcShort}(m)");
                var shortRebarResult = new CrackWidthTupleResult()
                {
                    CrackWidth = acrcShort,
                    UltimateCrackWidth = InputData.UserCrackInputData.UltimateShortCrackWidth
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

        private void GetSofteningLogic(RebarCrackInputData rebarData)
        {
            if (InputData.UserCrackInputData.SetSofteningFactor == true)
            {
                crackSofteningLogic = new StabSoftetingLogic(InputData.UserCrackInputData.SofteningFactor)
                {
                    TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
                };
            }
            else
            {
                crackSofteningLogic = new RebarStressSofteningLogic()
                {
                    RebarPrimitive = InputData.RebarPrimitive,
                    InputData = rebarData,
                    TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
                };
            }
        }

        private ICrackWidthLogicInputData GetCrackWidthInputData(RebarCrackInputData inputData, CalcTerms calcTerm)
        {
            
            var factoryInputData = new CrackWidthLogicInputDataFactory(crackSofteningLogic)
            {
                CalcTerm = calcTerm,
                InputData = inputData,
                RebarStrain = rebarStressResult.RebarStrain,
                ConcreteStrain = rebarStressResult.ConcreteStrain,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            var crackWidthInputData = factoryInputData.GetCrackWidthLogicInputData();
            return crackWidthInputData;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        private RebarStressResult GetRebarStressResult(RebarCrackInputData inputData)
        {
            var calculator = new RebarStressCalculator()
            {
                ForceTuple = inputData.ForceTuple,
                NdmCollection = inputData.CrackedNdmCollection,
                RebarPrimitive = InputData.RebarPrimitive
            };
            calculator.Run();
            var result = calculator.Result as RebarStressResult;
            if (result.IsValid == false)
            {
                string errorString = LoggerStrings.CalculationError + result.Description;
                TraceLogger?.AddMessage($"Rebar name: {InputData.RebarPrimitive.Name}\n" + errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            return result;
        }
    }
}
