using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureForceCalculator : ICurvatureForceCalculator
    {
        private CurvatureForceCalculatorResult result;
        public ICurvatureForceCalculatorInputData InputData { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public Guid Id => throw new NotImplementedException();

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            PrepareResult();
            if (CheckForCracks(InputData.ShortTermTuple, CalcTerms.ShortTerm) || CheckForCracks(InputData.ShortTermTuple, CalcTerms.ShortTerm))
            {
                ProcessCrackedSection();
            }
            else
            {
                ProcessUncrackedSection();
            }
        }

        private void ProcessCrackedSection()
        {
            throw new NotImplementedException();
        }

        private void ProcessUncrackedSection()
        {
            CurvatureTermCalculatorInputData inputData = new()
            {
                Primitives = InputData.Primitives,
                DeflectionFactor = InputData.DeflectionFactor
            };
            var calculator = new CurvatureTermUncrackedCalculator() { InputData = inputData };
            inputData.ForceTuple = InputData.LongTermTuple;
            inputData.CalculationTerm = CalcTerms.LongTerm;
            calculator.Run();
            var calcResult = calculator.Result as CurvatureTermCalculatorResult;
            if (calcResult.IsValid == false)
            {
                result.IsValid = false;
            }
            result.LongTermResult = calcResult;
        }

        private bool CheckForCracks(IForceTuple forceTuple, CalcTerms calcTerm)
        {
            var triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = InputData.Primitives,
                LimitState = LimitStates.SLS,
                CalcTerm = calcTerm,
                TraceLogger = TraceLogger
            };
            var ndms = triangulateLogic.GetNdms();
            var logic = new IsSectionCrackedByForceLogic()
            {
                ForceTuple = forceTuple,
                CheckedNdmCollection = ndms,
                SectionNdmCollection = ndms
            };
            return logic.IsSectionCracked();
        }

        private void PrepareResult()
        {
            result = new();
        }
    }
}
