using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
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
    public class CrackForceCalculator : ICalculator
    {
        static readonly CrackedLogic crackedLogic = new();
        ExpSofteningLogic softeningLogic = new();
        static readonly CrackStrainLogic crackStrainLogic = new();
        static readonly SofteningFactorLogic softeningFactorLogic = new();
        IForceTupleCalculator forceTupleCalculator;
        private CrackForceResult result;
        private FindParameterCalculator parameterCalculator;

        public string Name { get; set; }
        public IForceTuple StartTuple { get; set; }
        public IForceTuple EndTuple { get; set; }
        public IEnumerable<INdm> NdmCollection { get; set; }
        public Accuracy Accuracy {get;set; }
        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CrackForceCalculator(IForceTupleCalculator forceTupleCalculator)
        {
            StartTuple ??= new ForceTuple();
            Accuracy ??= new Accuracy() { IterationAccuracy = 0.0001d, MaxIterationCount = 10000 };
            this.forceTupleCalculator = forceTupleCalculator;
        }
        public CrackForceCalculator() : this(new ForceTupleCalculator())
        {
            
        }
        public void Run()
        {
            parameterCalculator = new FindParameterCalculator()
            {
                Accuracy = Accuracy,
                Predicate = crackedLogic.IsSectionCracked
            };
            if (TraceLogger is not null)
            {
                forceTupleCalculator.TraceLogger = TraceLogger.GetSimilarTraceLogger(100);
                parameterCalculator.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
                crackedLogic.TraceLogger = TraceLogger.GetSimilarTraceLogger(150);
            }
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLogStatuses.Service);
            result = new CrackForceResult();
            TraceLogger?.AddMessage($"Start force combination");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(StartTuple));
            TraceLogger?.AddMessage($"Actual (end) force combination");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(EndTuple));
            crackedLogic.StartTuple = StartTuple;
            crackedLogic.EndTuple = EndTuple;
            crackedLogic.NdmCollection = NdmCollection;
            try
            {
                Check();
            }
            catch(Exception ex)
            {
                result.IsValid = false;
                result.Description += ex;
                return;
            }
            if (crackedLogic.IsSectionCracked(0d) == true)
            {
                TraceLogger?.AddMessage($"Crack is appeared in start corce combination", TraceLogStatuses.Warning);
                SectionCrackedAtStart();
                return;
            }
            if (crackedLogic.IsSectionCracked(1d) == false)
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
            var tupleOfCrackApeearence = ForceTupleService.InterpolateTuples(EndTuple, StartTuple, factorOfCrackAppearance);
            TraceLogger?.AddMessage($"Crack is appeared in force combination");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(tupleOfCrackApeearence));
            var reducedStrainTuple = GetReducedStrainTuple(factorOfCrackAppearance, psiS);
            var crackedStrainTuple = GetStrainTuple(EndTuple);
            TraceLogger?.AddMessage($"Strains in cracked section from actual (end) force");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(crackedStrainTuple));
            TraceLogger?.AddMessage($"Average curvatures of cracked part of element");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(reducedStrainTuple));
            TraceLogger?.AddMessage($"Calculating factors of reducing of stifness");
            result.FactorOfCrackAppearance = factorOfCrackAppearance;
            result.IsValid = true;
            result.IsSectionCracked = true;
            result.Description += paramResult.Description;
            var softeningFactors = GetSofteningFactors(reducedStrainTuple);
            result.TupleOfCrackAppearance = tupleOfCrackApeearence;
            result.CrackedStrainTuple = crackedStrainTuple;
            result.ReducedStrainTuple = reducedStrainTuple;
            result.SofteningFactors= softeningFactors;
            result.PsiS = psiS;
            TraceLogger?.AddMessage($"Valid result was obtained", TraceLogStatuses.Debug);
        }

        private StrainTuple GetSofteningFactors(StrainTuple reducedStrainTuple)
        {
            softeningFactorLogic.NdmCollection = NdmCollection;
            softeningFactorLogic.StrainTuple = reducedStrainTuple;
            return softeningFactorLogic.GetSofteningFactors();
        }

        private StrainTuple GetReducedStrainTuple(double factorOfCrackAppearance, double softeningFactor)
        {
            const double notCrackedForceFactor = 0.99d;
            var notCrackedForceTuple = ForceTupleService.InterpolateTuples(EndTuple, StartTuple, factorOfCrackAppearance * notCrackedForceFactor) as ForceTuple;
            var crackAppearanceStrainTuple = GetStrainTuple(notCrackedForceTuple);
            var actualStrainTuple = GetStrainTuple(EndTuple);
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
            result.TupleOfCrackAppearance = (IForceTuple)StartTuple.Clone();
            softeningLogic.ForceRatio = result.FactorOfCrackAppearance;
            result.PsiS = softeningLogic.GetSofteningFactor();
            result.CrackedStrainTuple = result.ReducedStrainTuple = GetStrainTuple(EndTuple);
            result.SofteningFactors = GetSofteningFactors(result.ReducedStrainTuple);
            result.IsSectionCracked = true;
            result.Description += "Section cracked in start tuple";
        }
        private void SectionIsNotCracked()
        {
            result.IsValid = true;
            result.IsSectionCracked = false;
            result.CrackedStrainTuple = result.ReducedStrainTuple = GetStrainTuple(EndTuple);
            result.SofteningFactors = GetSofteningFactors(result.ReducedStrainTuple);
            result.Description = "Section is not cracked";
        }
        private StrainTuple GetStrainTuple(IForceTuple forceTuple)
        {
            ForceTupleInputData inputData = new();
            inputData.NdmCollection = NdmCollection;
            inputData.Tuple = forceTuple;
            forceTupleCalculator.InputData = inputData;
            forceTupleCalculator.Run();
            var result = forceTupleCalculator.Result as IForcesTupleResult;
            var loaderStrainMatrix = result.LoaderResults.ForceStrainPair.StrainMatrix;
            StrainTuple strainTuple = TupleConverter.ConvertToStrainTuple(loaderStrainMatrix);
            return strainTuple;
        }
        private void Check()
        {
            CheckObject.IsNull(EndTuple);
            if (StartTuple == EndTuple)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": Section is not cracked");
            }
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
