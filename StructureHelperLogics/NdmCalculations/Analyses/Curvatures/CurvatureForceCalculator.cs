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
        private ICurvatureTermCalculator calculator;

        public CurvatureForceCalculator(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

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
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLogStatuses.Service);
            PrepareResult();
            try
            {
                if (CheckForCracks(InputData.ForcePair.FullForceTuple, CalcTerms.ShortTerm) || CheckForCracks(InputData.ForcePair.LongForceTuple, CalcTerms.ShortTerm))
                {
                    TraceLogger?.AddMessage($"Section is cracked");
                    calculator = new CurvatureTermCrackedCalculator(TraceLogger) { InputData = InputData };
                }
                else
                {
                    TraceLogger?.AddMessage($"Section is not cracked");
                    calculator = new CurvatureTermUncrackedCalculator(TraceLogger) { InputData = InputData };
                }
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex.Message;
                result.Description += $"\n Check bearing capacity for action {InputData.ForcePair.Name}, probably section has been collapsed";
                return;
            }

            calculator.Run();
            var calcResult = calculator.Result as ICurvatureSectionResult;
            if (calcResult.IsValid == false)
            {
                result.IsValid = false;
            }
            result.SectionResult = calcResult;
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
            result = new()
            {
                InputData = InputData,
            };
        }
    }
}
