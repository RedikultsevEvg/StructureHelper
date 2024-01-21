using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackForceCalculator : ICalculator
    {
        static readonly CrackedLogic crackedLogic = new();
        static readonly ExpSofteningLogic softeningLogic = new();
        static readonly CrackStrainLogic crackStrainLogic = new();
        static readonly SofteningFactorLogic softeningFactorLogic = new();
        IForceTupleCalculator forceTupleCalculator;
        private CrackForceResult result;

        public string Name { get; set; }
        public ForceTuple StartTuple { get; set; }
        public ForceTuple EndTuple { get; set; }
        public IEnumerable<INdm> NdmCollection { get; set; }
        public Accuracy Accuracy {get;set; }
        public IResult Result => result;

        public ITraceLogger? TraceLogger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            result = new CrackForceResult();
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
                SectionCrackedAtStart();
                return;
            }
            if (crackedLogic.IsSectionCracked(1d) == false)
            {
                SectionIsNotCracked();
                return;
            }
            var parameterCalculator = new FindParameterCalculator()
            {
                Accuracy = Accuracy,
                Predicate = crackedLogic.IsSectionCracked
            };
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
            }
        }

        private void SectionIsCrackedBetween(FindParameterResult? paramResult)
        {
            var factorOfCrackAppearance = paramResult.Parameter;
            softeningLogic.ForceRatio = factorOfCrackAppearance;
            var psiS = softeningLogic.GetSofteningFactor();
            result.IsValid = true;
            result.IsSectionCracked = true;
            result.Description += paramResult.Description;
            result.FactorOfCrackAppearance = factorOfCrackAppearance;
            result.TupleOfCrackAppearance = ForceTupleService.InterpolateTuples(EndTuple, StartTuple, factorOfCrackAppearance);
            var reducedStrainTuple = GetReducedStrainTuple(factorOfCrackAppearance, psiS);
            result.CrackedStrainTuple = GetStrainTuple(EndTuple);
            result.ReducedStrainTuple = reducedStrainTuple;
            result.SofteningFactors=GetSofteningFactors(reducedStrainTuple);
            result.PsiS = psiS;
        }

        private StrainTuple GetSofteningFactors(StrainTuple reducedStrainTuple)
        {
            softeningFactorLogic.NdmCollection = NdmCollection;
            softeningFactorLogic.StrainTuple = reducedStrainTuple;
            return softeningFactorLogic.GetSofteningFactors();
        }

        private StrainTuple GetReducedStrainTuple(double factorOfCrackAppearance, double softeningFactor)
        {
            const double notCrackedFactor = 0.99d;
            var notCrackedForceTuple = ForceTupleService.InterpolateTuples(EndTuple, StartTuple, factorOfCrackAppearance * notCrackedFactor) as ForceTuple;
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
        private StrainTuple GetStrainTuple(ForceTuple forceTuple)
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
