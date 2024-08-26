using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarStressCalculator : IRebarStressCalculator
    {
        private const CalcTerms termOfLoadForCrackCalculation = CalcTerms.ShortTerm;
        private const LimitStates limitStateForCrackCalcultion = LimitStates.SLS;
        private IStressLogic stressLogic;
        private Ndm concreteNdm;
        private RebarNdm rebarNdm;
        private RebarStressResult result;

        public IRebarStressCalculatorInputData InputData { get; set; }
        public string Name { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }


        public RebarStressCalculator(IStressLogic stressLogic)
        {
            this.stressLogic = stressLogic;
            InputData = new RebarStressCalculatorInputData();
        }
        public RebarStressCalculator() : this(new StressLogic())
        {

        }


        public void Run()
        {
            PrepareNewResult();
            if (CheckInputData() != true)
            {
                return;
            }
            GetNdmCollectionByPrimitives();
            ProcessResult();
        }

        private bool CheckInputData()
        {
            return true;
        }

        private void ProcessResult()
        {
            var strainTuple = GetStrainTuple();
            result.StrainTuple = strainTuple;
            var strainMatrix = TupleConverter.ConvertToLoaderStrainMatrix(strainTuple);
            result.RebarStrain = stressLogic.GetSectionStrain(strainMatrix, rebarNdm);
            result.RebarStress = stressLogic.GetStress(strainMatrix, rebarNdm);
            result.ConcreteStrain = -concreteNdm.PrestrainLogic.GetAll().Sum(x => x.PrestrainValue);
        }

        private void PrepareNewResult()
        {
            result = new RebarStressResult()
            {
                IsValid = true,
                Description = string.Empty
            };
        }

        private StrainTuple GetStrainTuple()
        {
            IForceTupleInputData inputData = new ForceTupleInputData()
            {
                NdmCollection = InputData.NdmCollection,
                ForceTuple = InputData.ForceTuple
            };
            IForceTupleCalculator calculator = new ForceTupleCalculator()
            {
                InputData = inputData,
                //TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            calculator.Run();
            var forceResult = calculator.Result as IForcesTupleResult;
            if (forceResult.IsValid == false)
            {
                //TraceLogger?.AddMessage(LoggerStrings.CalculationError + $": {forceResult.Description}", TraceLogStatuses.Error);
                throw new StructureHelperException(ErrorStrings.CalculationError);
            }
            var strain = TupleConverter.ConvertToStrainTuple(forceResult.LoaderResults.StrainMatrix);
            return strain;
        }

        private void GetNdmCollectionByPrimitives()
        {
            var options = new TriangulationOptions()
            {
                CalcTerm = termOfLoadForCrackCalculation,
                LimiteState = limitStateForCrackCalcultion,
            };
            concreteNdm = InputData.RebarPrimitive.GetConcreteNdm(options);
            concreteNdm.StressScale = 1d;
            rebarNdm = InputData.RebarPrimitive.GetRebarNdm(options);
        }
        /// <inheritdoc/>
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
