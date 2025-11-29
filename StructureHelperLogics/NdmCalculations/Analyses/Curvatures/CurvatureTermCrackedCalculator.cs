using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureTermCrackedCalculator : ICurvatureTermCalculator
    {
        private CurvatureSectionResult result;
        private IGetTermDeflectionLogic deflectionLogic;
        private IGetTermDeflectionLogic DeflectionLogic => deflectionLogic ??= new GetTermDeflectionLogic(TraceLogger);
        private ICrackedSectionTriangulationLogic triangulationLogic;
        private List<INdm> crackableNdms;

        public CurvatureTermCrackedCalculator(IShiftTraceLogger? traceLogger)
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
            DeflectionLogic.DeflectionFactor = InputData.DeflectionFactor;
            PrepareNewResult();
            TraceLogger?.AddMessage($"Long term calculation");
            Triangulate(CalcTerms.LongTerm);
            ICrackForceResult? longCalcResult = GetCrackedResult(InputData.ForcePair.LongForceTuple);
            if (result.IsValid == false) { return; }
            DeflectionLogic.LongCurvature = longCalcResult.ReducedStrainTuple;
            result.LongTermResult = DeflectionLogic.GetLongResult();
            TraceLogger?.AddMessage($"Short term calculation");
            Triangulate(CalcTerms.ShortTerm);
            ICrackForceResult? shortFullCalcResult = GetCrackedResult(InputData.ForcePair.FullForceTuple);
            if (result.IsValid == false) { return; }
            DeflectionLogic.ShortFullCurvature = shortFullCalcResult.ReducedStrainTuple;
            ICrackForceResult? shortLongCalcResult = GetCrackedResult(InputData.ForcePair.LongForceTuple);
            if (result.IsValid == false) { return; }
            DeflectionLogic.ShortLongCurvature = shortLongCalcResult.ReducedStrainTuple;
            result.ShortTermResult = DeflectionLogic.GetShortResult();
        }

        private ICrackForceResult GetCrackedResult(IForceTuple forceTuple)
        {
            CrackForceCalculatorInputData inputData = new()
            {
                EndTuple = forceTuple,
                CheckedNdmCollection = crackableNdms,
                SectionNdmCollection = crackableNdms,
            };
            CrackForceBynarySearchCalculator calculator = new()
            {
                InputData = inputData,
                TraceLogger = TraceLogger
            };
            calculator.Run();
            var longCalcResult = calculator.Result as ICrackForceResult;
            if (longCalcResult.IsValid == false)
            {
                result.IsValid = false;
                result.Description += longCalcResult.Description;
            }

            return longCalcResult;
        }

        private void Triangulate(CalcTerms calcTerm)
        {
            triangulationLogic = new CrackedSectionTriangulationLogic(InputData.Primitives, calcTerm)
            {
                //TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            crackableNdms = triangulationLogic.GetNdmCollection();
        }

        private void PrepareNewResult()
        {
            result = new();
        }
    }
}
