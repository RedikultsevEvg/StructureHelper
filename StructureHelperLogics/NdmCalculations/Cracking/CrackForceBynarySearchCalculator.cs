using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackForceBynarySearchCalculator : ICrackForceCalculator
    {
        private IIsSectionCrackedByFactorLogic crackedByFactorLogic;
        private ICheckInputDataLogic<ICrackForceCalculatorInputData> checkInputDataLogic;
        ExpSofteningLogic softeningLogic = new();
        static readonly CrackStrainLogic crackStrainLogic = new();
        static readonly SofteningFactorLogic softeningFactorLogic = new();
        IForceTupleCalculator forceTupleCalculator;
        private CrackForceResult result;
        private FindParameterCalculator parameterCalculator;

        public string Name { get; set; }
        public ICrackForceCalculatorInputData InputData { get; set; }
        public Accuracy Accuracy { get; set; }
        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public Guid Id => throw new NotImplementedException();

        public CrackForceBynarySearchCalculator(
            IIsSectionCrackedByFactorLogic crackedByFactorLogic,
            ICheckInputDataLogic<ICrackForceCalculatorInputData> checkInputDataLogic
            )
        {
            this.crackedByFactorLogic = crackedByFactorLogic;
            this.checkInputDataLogic = checkInputDataLogic;
            Accuracy ??= new Accuracy()
            {
                IterationAccuracy = 0.0001d,
                MaxIterationCount = 10000
            };
            forceTupleCalculator = new ForceTupleCalculator();
            InputData = new CrackForceCalculatorInputData();
        }
        public CrackForceBynarySearchCalculator() : this(new IsSectionCrackedByFactorLogic(), new CheckCrackForceCalculatorInputDataLogic())
        {

        }
        public void Run()
        {
            PrepareNewResult();
            if (CheckInputData() == false) { return; }
            parameterCalculator = new FindParameterCalculator();
            parameterCalculator.InputData.Predicate = crackedByFactorLogic.IsSectionCracked;
            if (TraceLogger is not null)
            {
                forceTupleCalculator.TraceLogger = TraceLogger.GetSimilarTraceLogger(100);
                parameterCalculator.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
                crackedByFactorLogic.TraceLogger = TraceLogger.GetSimilarTraceLogger(150);
            }
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Start force combination");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(InputData.StartTuple));
            TraceLogger?.AddMessage($"Actual (end) force combination");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(InputData.EndTuple));
            crackedByFactorLogic.IsSectionCrackedByForceLogic = new IsSectionCrackedByForceLogic()
            {
                CheckedNdmCollection = InputData.CheckedNdmCollection,
                SectionNdmCollection = InputData.SectionNdmCollection,
            };
            crackedByFactorLogic.StartTuple = InputData.StartTuple;
            crackedByFactorLogic.EndTuple = InputData.EndTuple;
            try
            {
                CheckInputData();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex;
                return;
            }
            if (crackedByFactorLogic.IsSectionCracked(0d) == true)
            {
                TraceLogger?.AddMessage($"Crack is appeared in start force combination", TraceLogStatuses.Warning);
                SectionCrackedAtStart();
                return;
            }
            if (crackedByFactorLogic.IsSectionCracked(1d) == false)
            {
                TraceLogger?.AddMessage($"Crack is not appeared from actual (end) force combination", TraceLogStatuses.Warning);
                SectionIsNotCracked();
                return;
            }

            parameterCalculator.Run();
            var paramResult = parameterCalculator.Result as FindParameterResult;
            if (paramResult.IsValid == true)
            {
                SectionIsCrackedBetween(paramResult);
            }
            else
            {
                result.IsValid = false;
                result.Description += paramResult.Description;
                TraceLogger?.AddMessage($"Result of calculation is not valid\n{result.Description}", TraceLogStatuses.Error);
            }
        }

        private void PrepareNewResult()
        {
            result = new CrackForceResult();
        }

        private void SectionIsCrackedBetween(FindParameterResult? paramResult)
        {
            var factorOfCrackAppearance = paramResult.Parameter;
            softeningLogic = new ExpSofteningLogic();
            if (TraceLogger is not null)
            {
                softeningLogic.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
                softeningFactorLogic.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
            }
            softeningLogic.ForceRatio = factorOfCrackAppearance;
            var psiS = softeningLogic.GetSofteningFactor();
            var tupleOfCrackApeearence = ForceTupleService.InterpolateTuples(InputData.EndTuple, InputData.StartTuple, factorOfCrackAppearance);
            TraceLogger?.AddMessage($"Crack is appeared in force combination");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(tupleOfCrackApeearence));
            var reducedStrainTuple = GetReducedStrainTuple(factorOfCrackAppearance, psiS);
            var crackedStrainTuple = GetStrainTuple(InputData.EndTuple);
            TraceLogger?.AddMessage($"Strains in cracked section from actual (end) force");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(crackedStrainTuple));
            TraceLogger?.AddMessage($"Average strains of cracked part of element");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(reducedStrainTuple));
            TraceLogger?.AddMessage($"Factors of reducing of stiffness");
            result.FactorOfCrackAppearance = factorOfCrackAppearance;
            result.IsValid = true;
            result.IsSectionCracked = true;
            result.Description += paramResult.Description;
            var softeningFactors = GetSofteningFactors(reducedStrainTuple);
            result.TupleOfCrackAppearance = tupleOfCrackApeearence;
            result.CrackedStrainTuple = crackedStrainTuple;
            result.ReducedStrainTuple = reducedStrainTuple;
            result.SofteningFactors = softeningFactors;
            result.PsiS = psiS;
            TraceLogger?.AddMessage($"Valid result was obtained", TraceLogStatuses.Debug);
        }

        private StrainTuple GetSofteningFactors(StrainTuple reducedStrainTuple)
        {
            softeningFactorLogic.NdmCollection = InputData.SectionNdmCollection;
            softeningFactorLogic.StrainTuple = reducedStrainTuple;
            return softeningFactorLogic.GetSofteningFactors();
        }

        private StrainTuple GetReducedStrainTuple(double factorOfCrackAppearance, double softeningFactor)
        {
            const double notCrackedForceFactor = 0.99d;
            var notCrackedForceTuple = ForceTupleService.InterpolateTuples(InputData.EndTuple, InputData.StartTuple, factorOfCrackAppearance * notCrackedForceFactor) as ForceTuple;
            var crackAppearanceStrainTuple = GetStrainTuple(notCrackedForceTuple);
            var actualStrainTuple = GetStrainTuple(InputData.EndTuple);
            crackStrainLogic.BeforeCrackingTuple = crackAppearanceStrainTuple;
            crackStrainLogic.AfterCrackingTuple = actualStrainTuple;
            crackStrainLogic.SofteningFactor = softeningFactor;
            var reducedStrainTuple = crackStrainLogic.GetCrackedStrainTuple();
            return reducedStrainTuple is null
                ? throw new StructureHelperException(ErrorStrings.ResultIsNotValid + "\n Strain Tuple is null")
                : reducedStrainTuple;
        }

        private void SectionCrackedAtStart()
        {
            result.IsValid = true;
            result.FactorOfCrackAppearance = 0d;
            result.TupleOfCrackAppearance = (IForceTuple)InputData.StartTuple.Clone();
            softeningLogic.ForceRatio = result.FactorOfCrackAppearance;
            result.PsiS = softeningLogic.GetSofteningFactor();
            result.CrackedStrainTuple = result.ReducedStrainTuple = GetStrainTuple(InputData.EndTuple);
            result.SofteningFactors = GetSofteningFactors(result.ReducedStrainTuple);
            result.IsSectionCracked = true;
            result.Description += "Section cracked in start tuple";
        }
        private void SectionIsNotCracked()
        {
            result.IsValid = true;
            result.IsSectionCracked = false;
            result.CrackedStrainTuple = result.ReducedStrainTuple = GetStrainTuple(InputData.EndTuple);
            result.SofteningFactors = GetSofteningFactors(result.ReducedStrainTuple);
            result.Description = "Section is not cracked";
        }
        private StrainTuple GetStrainTuple(IForceTuple forceTuple)
        {
            ForceTupleInputData inputData = new();
            inputData.NdmCollection = InputData.SectionNdmCollection;
            inputData.ForceTuple = forceTuple;
            forceTupleCalculator.InputData = inputData;
            forceTupleCalculator.Run();
            var result = forceTupleCalculator.Result as IForcesTupleResult;
            var loaderStrainMatrix = result.LoaderResults.ForceStrainPair.StrainMatrix;
            StrainTuple strainTuple = TupleConverter.ConvertToStrainTuple(loaderStrainMatrix);
            return strainTuple;
        }
        private bool CheckInputData()
        {
            checkInputDataLogic.InputData = InputData;
            if (checkInputDataLogic.Check() == false)
            {
                result.IsValid = false;
                result.Description += checkInputDataLogic.CheckResult;
                return false;
            }
            return true;
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
