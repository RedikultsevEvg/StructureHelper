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
        private ICrackedSectionTriangulationLogic triangulationLogic;
        private List<RebarPrimitive>? rebarPrimitives;
        private IEnumerable<INdm> defaultNdms;
        private IEnumerable<INdm> fulyCrackedNdms;
        private IEnumerable<INdm> elasticNdms;
        private CrackForceResult crackForceResult;
        private StrainTuple longDefaultStrainTuple;
        private StrainTuple shortDefaultStrainTuple;
        private ITriangulatePrimitiveLogic triangulateLogic;
        private double longLength;
        private double shortLength;

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
                TraceLogger?.AddMessage($"Calculation of crack width by force combination");
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
            longDefaultStrainTuple = CalcStrainMatrix(InputData.LongTermTuple as ForceTuple, defaultNdms);
            shortDefaultStrainTuple = CalcStrainMatrix(InputData.LongTermTuple as ForceTuple, defaultNdms);
            var longElasticStrainTuple = CalcStrainMatrix(InputData.LongTermTuple as ForceTuple, elasticNdms);
            var shortElasticStrainTuple = CalcStrainMatrix(InputData.ShortTermTuple as ForceTuple, elasticNdms);
            if (result.IsValid == false) { return; }
            longLength = GetLengthBetweenCracks(longElasticStrainTuple);
            shortLength = GetLengthBetweenCracks(shortElasticStrainTuple);
            CalcCrackForce();
            foreach (var rebar in rebarPrimitives)
            {
                RebarCrackCalculatorInputData rebarCalculatorData = GetRebarCalculatorInputData(rebar);
                var calculator = new RebarCrackCalculator
                {
                    InputData = rebarCalculatorData,
                    TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
                };
                calculator.Run();
                var rebarResult = calculator.Result as RebarCrackResult;
                result.RebarResults.Add(rebarResult);
            }
        }

        private RebarCrackCalculatorInputData GetRebarCalculatorInputData(RebarPrimitive rebar)
        {
            var longRebarData = new RebarCrackInputData()
            {
                NdmCollection = fulyCrackedNdms,
                ForceTuple = InputData.LongTermTuple as ForceTuple,
                Length = longLength
            };
            var shortRebarData = new RebarCrackInputData()
            {
                NdmCollection = fulyCrackedNdms,
                ForceTuple = InputData.ShortTermTuple as ForceTuple,
                Length = shortLength
            };
            var rebarCalculatorData = new RebarCrackCalculatorInputData()
            {
                RebarPrimitive = rebar,
                LongRebarData = longRebarData,
                ShortRebarData = shortRebarData
            };
            return rebarCalculatorData;
        }

        private StrainTuple CalcStrainMatrix(ForceTuple forceTuple, IEnumerable<INdm> ndms)
        {
            IForceTupleInputData inputData = new ForceTupleInputData()
            {
                NdmCollection = ndms,
                Tuple = forceTuple
            };
            IForceTupleCalculator calculator = new ForceTupleCalculator()
            {
                InputData = inputData,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            calculator.Run();
            var forceResult = calculator.Result as IForcesTupleResult;
            if (forceResult.IsValid == false)
            {
                result.IsValid = false;
                result.Description += forceResult.Description;
            }
            var strain = TupleConverter.ConvertToStrainTuple(forceResult.LoaderResults.StrainMatrix);
            return strain;
        }

        private double GetLengthBetweenCracks(StrainTuple strainTuple)
        {
            var logic = new LengthBetweenCracksLogicSP63()
            {
                NdmCollection = elasticNdms,
                TraceLogger = TraceLogger
            };
            logic.StrainMatrix = TupleConverter.ConvertToLoaderStrainMatrix(strainTuple);
            return logic.GetLength();
        }

        private void Triangulate()
        {
            triangulationLogic = new CrackedSectionTriangulationLogic(InputData.NdmPrimitives)
            {
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            rebarPrimitives = triangulationLogic.GetRebarPrimitives();
            defaultNdms = triangulationLogic.GetNdmCollection();
            fulyCrackedNdms = triangulationLogic.GetCrackedNdmCollection();
            elasticNdms = triangulationLogic.GetElasticNdmCollection();
        }

        private void CalcCrackForce()
        {
            var calculator = new CrackForceCalculator();
            calculator.EndTuple = InputData.LongTermTuple;
            calculator.NdmCollection = defaultNdms;
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
