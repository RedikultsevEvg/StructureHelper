using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services.NdmPrimitives;

//Copyright (c) 2024 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class TupleCrackCalculator : ICalculator
    {
        private const CalcTerms crackingTerm = CalcTerms.ShortTerm;
        private const LimitStates crackingLimitState = LimitStates.SLS;
        private ILengthBetweenCracksLogic lengthLogic;
        private TupleCrackResult result;
        private ICrackedSectionTriangulationLogic triangulationLogic;
        private ITupleRebarsCrackSolver solver;
        private List<IRebarPrimitive>? rebarPrimitives;
        private IEnumerable<INdm> crackableNdms;
        private IEnumerable<INdm> crackedNdms;
        private IEnumerable<INdm> elasticNdms;
        private CrackForceResult crackForceResult;
        private StrainTuple longDefaultStrainTuple;
        private StrainTuple shortDefaultStrainTuple;
        private double longLength;
        private double shortLength;
        private ICheckInputDataLogic<TupleCrackInputData> checkInputDataLogic;

        public string Name { get; set; }
        public TupleCrackInputData InputData { get; set; }
        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public TupleCrackCalculator(ICheckInputDataLogic<TupleCrackInputData> checkInputDataLogic,
            ILengthBetweenCracksLogic lengthLogic, ICrackedSectionTriangulationLogic triangulationLogic, ITupleRebarsCrackSolver solver)
        {
            this.checkInputDataLogic = checkInputDataLogic;
            this.lengthLogic = lengthLogic;
            this.triangulationLogic = triangulationLogic;
            this.solver = solver;
        }

        public TupleCrackCalculator() : this (new CheckTupleCalculatorInputDataLogic(),
            new LengthBetweenCracksLogicSP63(),
            new CrackedSectionTriangulationLogic(),
            new TupleRebarsCrackSolver())
        {
            
        }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            PrepareNewResult();

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
                Description = string.Empty,
                InputData = InputData
            };
        }

        private void ProcessCalculations()
        {

            Triangulate();
            if (CheckInputData() == false)
            {
                return;
            }
            longDefaultStrainTuple = CalcStrainMatrix(InputData.LongTermTuple as ForceTuple, crackableNdms);
            shortDefaultStrainTuple = CalcStrainMatrix(InputData.LongTermTuple as ForceTuple, crackableNdms);
            GetLengthBeetwenCracks();
            SolveRebarResult();
            GetMinMaxCrackWidth();
        }

        private void GetLengthBeetwenCracks()
        {
            var longElasticStrainTuple = CalcStrainMatrix(InputData.LongTermTuple as ForceTuple, elasticNdms);
            var shortElasticStrainTuple = CalcStrainMatrix(InputData.ShortTermTuple as ForceTuple, elasticNdms);
            if (result.IsValid == false) { return; }
            if (InputData.UserCrackInputData.SetLengthBetweenCracks == true)
            {
                longLength = InputData.UserCrackInputData.LengthBetweenCracks;
                shortLength = InputData.UserCrackInputData.LengthBetweenCracks;
                TraceLogger?.AddMessage($"User value of length between cracks Lcrc = {longLength}");
            }
            else
            {
                longLength = GetLengthBetweenCracks(longElasticStrainTuple);
                shortLength = GetLengthBetweenCracks(shortElasticStrainTuple);
            }
        }

        private void GetMinMaxCrackWidth()
        {
            if (result.IsValid == false || result.RebarResults.Count == 0) { return; }
            result.LongTermResult = new()
            {
                CrackWidth = result.RebarResults.Max(x => x.LongTermResult.CrackWidth),
                UltimateCrackWidth = InputData.UserCrackInputData.UltimateLongCrackWidth
            };
            result.ShortTermResult = new()
            {
                CrackWidth = result.RebarResults.Max(x => x.ShortTermResult.CrackWidth),
                UltimateCrackWidth = InputData.UserCrackInputData.UltimateShortCrackWidth
            };
        }

        private void SolveRebarResult()
        {
            result.RebarResults.Clear();
            solver.Rebars = rebarPrimitives;
            solver.InputData = InputData;
            solver.LongLength = longLength;
            solver.ShortLength = shortLength;
            solver.TraceLogger = TraceLogger?.GetSimilarTraceLogger(0);
            solver.Run();
            if (solver.IsResultValid == false)
            {
                result.IsValid = false;
                result.Description += solver.Description;
                return;
            }
            result.RebarResults.AddRange(solver.Result);
        }

        private StrainTuple CalcStrainMatrix(ForceTuple forceTuple, IEnumerable<INdm> ndms)
        {
            IForceTupleInputData inputData = new ForceTupleInputData()
            {
                NdmCollection = ndms,
                ForceTuple = forceTuple
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
                result.IsValid = false;
                result.Description += forceResult.Description;
                TraceLogger?.AddMessage("Bearing capacity of cross-section is not enough for action", TraceLogStatuses.Error);
                return null;
            }
            var strain = TupleConverter.ConvertToStrainTuple(forceResult.LoaderResults.StrainMatrix);
            return strain;
        }

        private double GetLengthBetweenCracks(StrainTuple strainTuple)
        {
            lengthLogic.NdmCollection = elasticNdms;
            lengthLogic.TraceLogger = TraceLogger;
            lengthLogic.StrainMatrix = TupleConverter.ConvertToLoaderStrainMatrix(strainTuple);
            return lengthLogic.GetLength();
        }

        private void Triangulate()
        {
            triangulationLogic = new CrackedSectionTriangulationLogic(InputData.Primitives)
            {
                //TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            rebarPrimitives = triangulationLogic.GetRebarPrimitives();
            crackableNdms = triangulationLogic.GetNdmCollection();
            crackedNdms = triangulationLogic.GetCrackedNdmCollection();
            elasticNdms = triangulationLogic.GetElasticNdmCollection();
        }

        private bool CheckInputData()
        {
            checkInputDataLogic.InputData = InputData;
            if (checkInputDataLogic.Check() == false)
            {
                result.IsValid = false;
                result.Description += checkInputDataLogic.CheckResult;
                TraceLogger?.AddMessage($"Input data is not correct: {checkInputDataLogic.CheckResult}", TraceLogStatuses.Error);
                return false;
            };
            return true;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
