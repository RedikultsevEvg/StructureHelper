using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthCalculationLogic : ICrackWidthCalculationLogic
    {
        private IRebarStressResultLogic rebarStressResultLogic;
        private ICrackWidthLogic crackWidthLogic;
        private RebarCrackResult result;
        private ICrackSofteningLogic crackSofteningLogic;
        private IRebarStressResult rebarStressResult;
        private ICrackWidthLogicInputData acrc2InputData;
        private ICrackWidthLogicInputData acrc1InputData;
        private ICrackWidthLogicInputData acrc3InputData;
        /// <summary>
        /// Width of crack from long term loads with long term properties of concrete
        /// </summary>
        private double longTermLoadLongTermConcreteCrackWidth;
        /// <summary>
        /// Width of crack from full (include short term) loads with short term properties of concrete
        /// </summary>
        private double fullLoadShortConcreteCrackWidth;
        /// <summary>
        /// Width of crack from long term loads with short term properties of concrete
        /// </summary>
        private double longTermLoadShortConcreteCrackWidth;
        private IRebarStressCalculator rebarStressCalculator;

        public IRebarCrackCalculatorInputData InputData { get; set; }
        public RebarCrackResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CrackWidthCalculationLogic(IRebarStressResultLogic rebarStressResultLogic, ICrackWidthLogic crackWidthLogic, IShiftTraceLogger? traceLogger)
        {
            this.rebarStressResultLogic = rebarStressResultLogic;
            this.crackWidthLogic = crackWidthLogic;
            this.TraceLogger = traceLogger;
        }

        public CrackWidthCalculationLogic() : this (new RebarStressResultLogic(), new CrackWidthLogicSP63(), null)
        {
            
        }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            PrepareNewResult();
            ProcessCrackWidthCalculation();
        }

        private void PrepareNewResult()
        {
            result = new()
            {
                IsValid = true,
                Description = string.Empty,
            };
        }

        private void ProcessCrackWidthCalculation()
        {
            crackWidthLogic.TraceLogger = TraceLogger;
            CrackWidthRebarTupleResult longRebarResult = ProcessLongTermCalculations();
            CrackWidthRebarTupleResult shortRebarResult = ProcessShortTermCalculations();
            result.LongTermResult = longRebarResult;
            TraceLogger?.AddMessage("Long term result has been obtained succesfully", TraceLogStatuses.Debug);
            result.ShortTermResult = shortRebarResult;
            TraceLogger?.AddMessage("Short term result has been obtained succesfully", TraceLogStatuses.Debug);
        }

        private CrackWidthRebarTupleResult ProcessShortTermCalculations()
        {
            TraceLogger?.AddMessage($"Short term softening factor calculation");
            crackSofteningLogic = GetSofteningLogic(InputData.ShortRebarData);
            rebarStressResult = GetRebarStressResult(InputData.ShortRebarData);
            acrc3InputData = GetCrackWidthInputData(crackSofteningLogic, InputData.LongRebarData, CalcTerms.ShortTerm);
            crackWidthLogic.InputData = acrc3InputData;
            longTermLoadShortConcreteCrackWidth = crackWidthLogic.GetCrackWidth();
            TraceLogger?.AddMessage($"Crack width from long term load with short term factor of concrete acrc,3 = {longTermLoadShortConcreteCrackWidth}(m)", TraceLogStatuses.Debug);
            acrc2InputData = GetCrackWidthInputData(crackSofteningLogic, InputData.ShortRebarData, CalcTerms.ShortTerm);
            crackWidthLogic.InputData = acrc2InputData;
            fullLoadShortConcreteCrackWidth = crackWidthLogic.GetCrackWidth();
            TraceLogger?.AddMessage($"Crack width from full load with short term factor of concrete acrc,2 = {fullLoadShortConcreteCrackWidth}(m)", TraceLogStatuses.Debug);

            double acrcShort = longTermLoadLongTermConcreteCrackWidth + fullLoadShortConcreteCrackWidth - longTermLoadShortConcreteCrackWidth;
            TraceLogger?.AddMessage($"Short crack width acrc = acrc,1 + acrc,2 - acrc,3 = {longTermLoadLongTermConcreteCrackWidth} + {fullLoadShortConcreteCrackWidth} - {longTermLoadShortConcreteCrackWidth} = {acrcShort}(m)");
            var shortRebarResult = new CrackWidthRebarTupleResult()
            {
                CrackWidth = acrcShort,
                UltimateCrackWidth = InputData.UserCrackInputData.UltimateShortCrackWidth,
                RebarStressResult = rebarStressResult,
                SofteningFactor = crackSofteningLogic.GetSofteningFactor()
            };
            TraceCrackResult(shortRebarResult);
            return shortRebarResult;
        }

        private CrackWidthRebarTupleResult ProcessLongTermCalculations()
        {
            TraceLogger?.AddMessage($"Long term softening factor calculation");
            crackSofteningLogic = GetSofteningLogic(InputData.LongRebarData);
            rebarStressResult = GetRebarStressResult(InputData.LongRebarData);
            acrc1InputData = GetCrackWidthInputData(crackSofteningLogic, InputData.LongRebarData, CalcTerms.LongTerm);
            crackWidthLogic.InputData = acrc1InputData;
            longTermLoadLongTermConcreteCrackWidth = crackWidthLogic.GetCrackWidth();
            TraceLogger?.AddMessage($"Crack width from long term load with long term factor of concrete acrc,1 = {longTermLoadLongTermConcreteCrackWidth}(m)", TraceLogStatuses.Debug);
            var longRebarResult = new CrackWidthRebarTupleResult()
            {
                CrackWidth = longTermLoadLongTermConcreteCrackWidth,
                UltimateCrackWidth = InputData.UserCrackInputData.UltimateLongCrackWidth,
                RebarStressResult = rebarStressResult,
                SofteningFactor = crackSofteningLogic.GetSofteningFactor()
            };
            TraceLogger?.AddMessage($"Long crack width acrc = acrc,1 = {longTermLoadLongTermConcreteCrackWidth}(m)");
            TraceLogger?.AddMessage($"Ultimate long crack width acrc,ult = {longRebarResult.UltimateCrackWidth}(m)");
            TraceCrackResult(longRebarResult);
            return longRebarResult;
        }

        private void TraceCrackResult(CrackWidthRebarTupleResult rebarResult)
        {
            if (rebarResult.IsCrackLessThanUltimate == false)
            {
                TraceLogger?.AddMessage($"Checking crack width is failure, actual crack width acrc = {rebarResult.CrackWidth} > ultimate crack width acrc,ult = {rebarResult.UltimateCrackWidth}", TraceLogStatuses.Warning);
            }
            else
            {
                TraceLogger?.AddMessage($"Checking crack width is ok, actual crack width acrc = {rebarResult.CrackWidth} <= ultimate crack width acrc,ult = {rebarResult.UltimateCrackWidth}");
            }
        }

        private ICrackSofteningLogic GetSofteningLogic(IRebarCrackInputData rebarData)
        {
            ICrackSofteningLogic crackSofteningLogic;
            if (InputData.UserCrackInputData.SetSofteningFactor == true)
            {
                TraceLogger?.AddMessage("User value of softening factor is assigned");
                crackSofteningLogic = new StabSoftetingLogic(InputData.UserCrackInputData.SofteningFactor)
                {
                    TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
                };
            }
            else
            {
                TraceLogger?.AddMessage("Exact value of softening factor is calculated");
                crackSofteningLogic = new RebarStressSofteningLogic()
                {
                    RebarPrimitive = InputData.RebarPrimitive,
                    InputData = rebarData,
                    TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
                };
            }
            return crackSofteningLogic;
        }

        private ICrackWidthLogicInputData GetCrackWidthInputData(ICrackSofteningLogic crackSofteningLogic, IRebarCrackInputData inputData, CalcTerms calcTerm)
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

        private IRebarStressResult GetRebarStressResult(IRebarCrackInputData rebarCrackInputData)
        {
            rebarStressResultLogic.RebarCrackInputData = rebarCrackInputData;
            rebarStressResultLogic.RebarPrimitive = InputData.RebarPrimitive;
            rebarStressResultLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            return rebarStressResultLogic.GetRebarStressResult();
        }

    }
}
