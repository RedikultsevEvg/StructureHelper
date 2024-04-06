using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthCalculator : ICalculator
    {
        private static readonly ILengthBetweenCracksLogic lengthLogic = new LengthBetweenCracksLogicSP63();
        private CrackWidthCalculatorResult result;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private List<RebarPrimitive>? rebarPrimitives;
        private IEnumerable<INdm> ndmCollection;
        private CrackForceResult crackForceResult;
        private StrainTuple strainTuple;

        public string Name { get; set; }
        public CrackWidthCalculatorInputData InputData { get; set; }
        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

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
                //if (crackForceResult.IsSectionCracked == false)
                //{
                //    rebarResult.CrackWidth = 0d;
                //}
                result.RebarResults.Add(rebarResult);
            }
        }

        private void CalcStrainMatrix()
        {
            IForceTupleInputData inputData = new ForceTupleInputData() { NdmCollection = ndmCollection, Tuple = InputData.LongTermTuple};
            IForceTupleCalculator calculator = new ForceTupleCalculator() { InputData = inputData };
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
            ndmCollection = NdmPrimitivesService.GetNdms(ndmPrimitives, LimitStates.SLS, CalcTerms.ShortTerm);
        }

        private void CalcCrackForce()
        {
            var calculator = new CrackForceCalculator();
            calculator.EndTuple = InputData.LongTermTuple;
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
