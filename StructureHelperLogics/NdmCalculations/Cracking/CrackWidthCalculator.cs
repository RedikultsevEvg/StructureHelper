using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthCalculator : ICalculator
    {
        static readonly ILengthBetweenCracksLogic lengthLogic = new LengthBetweenCracksLogicSP63();
        CrackWidthCalculatorResult result;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private List<RebarPrimitive>? rebarPrimitives;
        private IEnumerable<INdm> ndmCollection;
        private CrackForceResult crackForceResult;
        private StrainTuple strainTuple;

        public string Name { get; set; }
        public CrackWidthCalculatorInputData InputData { get; set; }
        public IResult Result => result;

        public ITraceLogger? TraceLogger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Run()
        {
            result = new() { IsValid = true, Description = ""};
            try
            {
                ProcessCalculations();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex;
            }

        }

        private void ProcessCalculations()
        {
            CheckInputData();
            Triangulate();
            CalcStrainMatrix();
            CalcCrackForce();
            var crackInputData = GetCrackInputData();
            var calculator = new CrackWidthSimpleCalculator { InputData = crackInputData };
            foreach (var item in rebarPrimitives)
            {
                crackInputData.RebarPrimitive = item;
                calculator.Run();
                var rebarResult = calculator.Result as CrackWidthSimpleCalculatorResult;
                if (crackForceResult.IsSectionCracked == false)
                {
                    rebarResult.CrackWidth = 0d;
                }
                result.RebarResults.Add(rebarResult);
            }
        }

        private void CalcStrainMatrix()
        {
            IForceTupleInputData inputData = new ForceTupleInputData() { NdmCollection = ndmCollection, Tuple = InputData.ForceTuple};
            IForceTupleCalculator calculator = new ForceTupleCalculator(inputData);
            calculator.Run();
            var forceResult = calculator.Result as IForcesTupleResult;
            strainTuple = TupleConverter.ConvertToStrainTuple(forceResult.LoaderResults.StrainMatrix);
        }

        private CrackWidthSimpleCalculatorInputData GetCrackInputData()
        {
            lengthLogic.NdmCollection = ndmCollection;
            lengthLogic.StrainMatrix = TupleConverter.ConvertToLoaderStrainMatrix(strainTuple);
            var length = lengthLogic.GetLength();
            var crackInputData = new CrackWidthSimpleCalculatorInputData
            {
                PsiSFactor = crackForceResult.PsiS,
                Length = length,
                LimitState = InputData.LimitState,
                StrainTuple = strainTuple
            };
            return crackInputData;
        }

        private void Triangulate()
        {
            ndmPrimitives = InputData.NdmPrimitives;
            rebarPrimitives = new List<RebarPrimitive>();
            foreach (var item in ndmPrimitives)
            {
                if (item is RebarPrimitive)
                {
                    rebarPrimitives.Add(item as RebarPrimitive);
                }
            }
            ndmCollection = NdmPrimitivesService.GetNdms(ndmPrimitives, InputData.LimitState, InputData.CalcTerm);
        }

        private void CalcCrackForce()
        {
            var calculator = new CrackForceCalculator();
            calculator.EndTuple = InputData.ForceTuple;
            calculator.NdmCollection = ndmCollection;
            calculator.Run();
            crackForceResult = calculator.Result as CrackForceResult;
        }

        private void CheckInputData()
        {
            if (InputData.NdmPrimitives is null || InputData.NdmPrimitives.Count == 0)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": input data doesn't have any primitives");
            }    
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
