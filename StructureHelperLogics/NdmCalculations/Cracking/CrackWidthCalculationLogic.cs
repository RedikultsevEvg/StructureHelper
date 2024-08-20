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
        private double longTermLoadShortConcreteWidth;
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
            CrackWidthRebarTupleResult longRebarResult = ProcessLongTermCalculations();
            CrackWidthRebarTupleResult shortRebarResult = ProcessShortTermCalculations();
            result.LongTermResult = longRebarResult;
            result.ShortTermResult = shortRebarResult;
        }

        private CrackWidthRebarTupleResult ProcessShortTermCalculations()
        {
            crackSofteningLogic = GetSofteningLogic(InputData.ShortRebarData);
            rebarStressResult = GetRebarStressResult(InputData.ShortRebarData);
            acrc2InputData = GetCrackWidthInputData(crackSofteningLogic, InputData.ShortRebarData, CalcTerms.ShortTerm);

            crackWidthLogic.InputData = acrc3InputData;
            longTermLoadShortConcreteWidth = crackWidthLogic.GetCrackWidth();
            crackWidthLogic.InputData = acrc2InputData;
            fullLoadShortConcreteCrackWidth = crackWidthLogic.GetCrackWidth();

            double acrcShort = longTermLoadLongTermConcreteCrackWidth + fullLoadShortConcreteCrackWidth - longTermLoadShortConcreteWidth;
            TraceLogger?.AddMessage($"Short crack width acrc = acrc,1 + acrc,2 - acrc,3 = {longTermLoadLongTermConcreteCrackWidth} + {fullLoadShortConcreteCrackWidth} - {longTermLoadShortConcreteWidth} = {acrcShort}(m)");
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
            crackSofteningLogic = GetSofteningLogic(InputData.LongRebarData);
            rebarStressResult = GetRebarStressResult(InputData.LongRebarData);
            acrc1InputData = GetCrackWidthInputData(crackSofteningLogic, InputData.LongRebarData, CalcTerms.LongTerm);
            acrc3InputData = GetCrackWidthInputData(crackSofteningLogic, InputData.LongRebarData, CalcTerms.ShortTerm);
            crackWidthLogic.InputData = acrc1InputData;
            longTermLoadLongTermConcreteCrackWidth = crackWidthLogic.GetCrackWidth();
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
