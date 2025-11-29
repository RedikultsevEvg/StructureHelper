using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureTermUncrackedCalculator : ICurvatureTermCalculator
    {
        private CurvatureSectionResult result;
        private IGetTermDeflectionLogic deflectionLogic;
        private IGetTermDeflectionLogic DeflectionLogic => deflectionLogic ??= new GetTermDeflectionLogic(TraceLogger);
        private List<INdm> longTermNdms;
        private List<INdm> shortTermNdms;

        public CurvatureTermUncrackedCalculator(IShiftTraceLogger? traceLogger)
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
            PrepareNewResult();
            TriangulatePrimitives();
            Calculate();
        }

        private void TriangulatePrimitives()
        {
            longTermNdms = TriangulatePrimitives(CalcTerms.LongTerm);
            shortTermNdms = TriangulatePrimitives(CalcTerms.ShortTerm);
        }

        private void Calculate()
        {
            try
            {
                DeflectionLogic.DeflectionFactor = InputData.DeflectionFactor;
                IForceTupleCalculatorResult? longCalcResult = GetCalculatorResult(longTermNdms, InputData.ForcePair.LongForceTuple);
                if (CheckCalcResul(longCalcResult) == false) { return; }
                DeflectionLogic.LongCurvature = ForceTupleConverter.ConvertToForceTuple(longCalcResult.LoaderResults.StrainMatrix);
                result.LongTermResult = DeflectionLogic.GetLongResult();

                IForceTupleCalculatorResult? shortFullCalcResult = GetCalculatorResult(shortTermNdms, InputData.ForcePair.FullForceTuple);
                if (CheckCalcResul(shortFullCalcResult) == false) { return; }
                DeflectionLogic.ShortFullCurvature = ForceTupleConverter.ConvertToForceTuple(shortFullCalcResult.LoaderResults.StrainMatrix);

                IForceTupleCalculatorResult? shortLongCalcResult = GetCalculatorResult(shortTermNdms, InputData.ForcePair.LongForceTuple);
                if (CheckCalcResul(shortLongCalcResult) == false) { return; }
                DeflectionLogic.ShortLongCurvature = ForceTupleConverter.ConvertToForceTuple(shortLongCalcResult.LoaderResults.StrainMatrix);
                result.ShortTermResult = DeflectionLogic.GetShortResult();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description = ex.Message;
            }
        }

        private bool CheckCalcResul(IForceTupleCalculatorResult? calcResult)
        {
            if (calcResult.IsValid != true)
            {
                result.IsValid = false;
                result.Description += calcResult.Description;
                return false;
            }
            return true;
        }

        private IForceTupleCalculatorResult? GetCalculatorResult(List<INdm> ndms, IForceTuple forceTuple)
        {
            var inputData = new ForceTupleInputData()
            {
                NdmCollection = ndms,
                ForceTuple = forceTuple
            };
            var calculator = new ForceTupleCalculator()
            {
                InputData = inputData,
            };
            calculator.Run();
            var calcResult = calculator.Result as IForceTupleCalculatorResult;
            return calcResult;
        }


        private List<INdm> TriangulatePrimitives(CalcTerms calcTerm)
        {
            
            var triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = InputData.Primitives,
                LimitState = LimitStates.SLS,
                CalcTerm = calcTerm,
                TraceLogger = TraceLogger
            };
            return triangulateLogic.GetNdms();
        }

        private void PrepareNewResult()
        {
            result = new();
        }
    }
}
