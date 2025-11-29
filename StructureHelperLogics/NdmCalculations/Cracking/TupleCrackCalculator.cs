using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;

//Copyright (c) 2024 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class TupleCrackCalculator : ILogicCalculator
    {
        private const CalcTerms crackingTerm = CalcTerms.ShortTerm;
        private const LimitStates crackingLimitState = LimitStates.SLS;
        private TupleCrackResult result;
        private ICrackedSectionTriangulationLogic triangulationLogic;
        private List<IRebarNdmPrimitive>? rebarPrimitives;
        private IEnumerable<INdm> crackableNdms;
        private IEnumerable<INdm> crackedNdms;
        private IEnumerable<INdm> elasticNdms;
        private CrackForceResult crackForceResult;
        private StrainTuple longDefaultStrainTuple;
        private StrainTuple shortDefaultStrainTuple;
        private double longLength;
        private double shortLength;
        private ICheckInputDataLogic<TupleCrackInputData> checkInputDataLogic;
        private ILengthBetweenCracksLogic lengthLogic;
        private ITupleRebarsCrackSolver crackSolver;
        private ICheckInputDataLogic<TupleCrackInputData> CheckInputDataLogic => checkInputDataLogic ??= new CheckTupleCalculatorInputDataLogic();
        private ILengthBetweenCracksLogic LengthLogic => lengthLogic ??= new LengthBetweenCracksLogicSP63();
        private ITupleRebarsCrackSolver CrackSolver => crackSolver ??= new TupleRebarsCrackSolver();

        public TupleCrackInputData InputData { get; set; }
        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public Guid Id => throw new NotImplementedException();

        public TupleCrackCalculator(ICheckInputDataLogic<TupleCrackInputData> checkInputDataLogic,
            ILengthBetweenCracksLogic lengthLogic, ICrackedSectionTriangulationLogic triangulationLogic, ITupleRebarsCrackSolver solver)
        {
            this.checkInputDataLogic = checkInputDataLogic;
            this.lengthLogic = lengthLogic;
            this.triangulationLogic = triangulationLogic;
            this.crackSolver = solver;
        }

        public TupleCrackCalculator() { }

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
            CrackSolver.Rebars = rebarPrimitives;
            CrackSolver.InputData = InputData;
            CrackSolver.LongLength = longLength;
            CrackSolver.ShortLength = shortLength;
            CrackSolver.TraceLogger = TraceLogger?.GetSimilarTraceLogger(0);
            CrackSolver.Run();
            if (CrackSolver.IsResultValid == false)
            {
                result.IsValid = false;
                result.Description += CrackSolver.Description;
                return;
            }
            result.RebarResults.AddRange(CrackSolver.Result);
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
            var forceResult = calculator.Result as IForceTupleCalculatorResult;
            if (forceResult.IsValid == false)
            {
                result.IsValid = false;
                result.Description += forceResult.Description;
                TraceLogger?.AddMessage("Bearing capacity of cross-section is not enough for action", TraceLogStatuses.Error);
                return null;
            }
            var strain = ForceTupleConverter.ConvertToStrainTuple(forceResult.LoaderResults.StrainMatrix);
            return strain;
        }

        private double GetLengthBetweenCracks(StrainTuple strainTuple)
        {
            LengthLogic.NdmCollection = elasticNdms;
            LengthLogic.TraceLogger = TraceLogger;
            LengthLogic.StrainMatrix = ForceTupleConverter.ConvertToLoaderStrainMatrix(strainTuple);
            return LengthLogic.GetLength();
        }

        private void Triangulate()
        {
            triangulationLogic = new CrackedSectionTriangulationLogic(InputData.Primitives, crackingTerm)
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
            CheckInputDataLogic.InputData = InputData;
            if (CheckInputDataLogic.Check() == false)
            {
                result.IsValid = false;
                result.Description += CheckInputDataLogic.CheckResult;
                TraceLogger?.AddMessage($"Input data is not correct: {CheckInputDataLogic.CheckResult}", TraceLogStatuses.Error);
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
