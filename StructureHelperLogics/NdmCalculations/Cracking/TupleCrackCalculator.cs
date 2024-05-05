using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class TupleCrackCalculator : ICalculator
    {
        private static readonly ILengthBetweenCracksLogic lengthLogic = new LengthBetweenCracksLogicSP63();
        private TupleCrackResult result;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private List<RebarPrimitive>? rebarPrimitives;
        private IEnumerable<INdm> defaultNdm;
        private IEnumerable<INdm> fulyCrackedNdm;
        private CrackForceResult crackForceResult;
        private StrainTuple strainTuple;
        private ITriangulatePrimitiveLogic triangulateLogic;

        public string Name { get; set; }
        public TupleCrackInputData InputData { get; set; }
        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public void Run()
        {
            PrepareNewResult();
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);

            try
            {
                ProcessCalculations();
                TraceLogger?.AddMessage(LoggerStrings.CalculationHasDone);

            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex;
                TraceLogger?.AddMessage(LoggerStrings.CalculationError + ex, TraceLogStatuses.Error);

            }

        }

        private void PrepareNewResult()
        {
            result = new()
            {
                IsValid = true,
                Description = string.Empty
            };
        }

        private void ProcessCalculations()
        {
            CheckInputData();
            Triangulate();
            CalcStrainMatrix();
            CalcCrackForce();
            var crackInputData = GetCrackInputData();
            var calculator = new RebarCrackCalculator
            {
                InputData = crackInputData,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            foreach (var item in rebarPrimitives)
            {
                crackInputData.RebarPrimitive = item;
                calculator.Run();
                var rebarResult = calculator.Result as RebarCrackResult;
                //if (crackForceResult.IsSectionCracked == false)
                //{
                //    rebarResult.CrackWidth = 0d;
                //}
                result.RebarResults.Add(rebarResult);
            }
        }

        private void CalcStrainMatrix()
        {
            IForceTupleInputData inputData = new ForceTupleInputData() { NdmCollection = defaultNdm, Tuple = InputData.LongTermTuple};
            IForceTupleCalculator calculator = new ForceTupleCalculator() { InputData = inputData };
            calculator.Run();
            var forceResult = calculator.Result as IForcesTupleResult;
            strainTuple = TupleConverter.ConvertToStrainTuple(forceResult.LoaderResults.StrainMatrix);
        }

        private RebarCrackInputData GetCrackInputData()
        {
            lengthLogic.NdmCollection = defaultNdm;
            lengthLogic.StrainMatrix = TupleConverter.ConvertToLoaderStrainMatrix(strainTuple);
            var length = lengthLogic.GetLength();
            var crackInputData = new RebarCrackInputData
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
            triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = ndmPrimitives,
                LimitState = LimitStates.SLS,
                CalcTerm = CalcTerms.ShortTerm,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
        };
            defaultNdm = triangulateLogic.GetNdms();
        }

        private void CalcCrackForce()
        {
            var calculator = new CrackForceCalculator();
            calculator.EndTuple = InputData.LongTermTuple;
            calculator.NdmCollection = defaultNdm;
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
