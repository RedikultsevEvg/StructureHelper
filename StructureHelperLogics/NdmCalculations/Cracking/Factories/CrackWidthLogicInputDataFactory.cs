using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Loggers;
using LoaderCalculator.Data.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    internal class CrackWidthLogicInputDataFactory : ILogic
    {
        private IStressLogic stressLogic => new StressLogic();

        private const double minimumPsiSFactor = 0.2d;
        private INdm concreteNdm;
        private INdm rebarNdm;
        private StrainTuple strainTupleActual;
        private double rebarStrainActual;
        private double concreteStrainActual;
        private double rebarStressActual;

        public CalcTerms CalcTerm { get; set; }
        public RebarPrimitive RebarPrimitive { get; set; }
        public RebarCrackInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public ICrackWidthLogicInputData GetCrackWidthLogicInputData()
        {
            GetNdms();
            CrackWidthLogicInputDataSP63 data = new();
            if (CalcTerm == CalcTerms.LongTerm)
            {
                data.TermFactor = 1.4d;
            }
            else
            {
                data.TermFactor = 1d;
            }
            data.PsiSFactor = GetPsiSFactor(InputData.ForceTuple, InputData.NdmCollection);
            data.StressStateFactor = 1.0d;
            data.BondFactor = 0.5d;
            data.Length = InputData.Length;
            data.ConcreteStrain = concreteStrainActual;
            data.RebarStrain = rebarStrainActual;
            return data;
        }

        private void GetNdms()
        {
            var options = new TriangulationOptions()
            {
                CalcTerm = CalcTerms.ShortTerm,
                LimiteState = LimitStates.SLS,
            };
            concreteNdm = RebarPrimitive.GetConcreteNdm(options);
            concreteNdm.StressScale = 1d;
            rebarNdm = RebarPrimitive.GetRebarNdm(options);
        }

        private double GetPsiSFactor(ForceTuple forceTuple, IEnumerable<INdm> ndms)
        {
            var crackResult = calculateCrackTuples(forceTuple, ndms);
            strainTupleActual = CalcStrainMatrix(forceTuple, ndms);
            rebarStrainActual = stressLogic.GetTotalStrain(TupleConverter.ConvertToLoaderStrainMatrix(strainTupleActual), rebarNdm);
            TraceLogger?.AddMessage($"Actual strain of rebar EpsilonS = {rebarStrainActual}(dimensionless)");
            concreteStrainActual = concreteNdm.Prestrain;
            //concreteStrainActual = stressLogic.GetTotalStrain(TupleConverter.ConvertToLoaderStrainMatrix(strainTupleActual), concreteNdm);
            TraceLogger?.AddMessage($"Actual strain of concrete on the axis of rebar EpsilonC = {concreteStrainActual}(dimensionless");
            if (crackResult.IsValid == false)
            {
                string errorString = LoggerStrings.CalculationError + crackResult.Description;
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            if (crackResult.IsSectionCracked == false)
            {
                TraceLogger?.AddMessage($"Section is not cracked PsiS = {minimumPsiSFactor}");
                return minimumPsiSFactor;
            }
            if (crackResult.FactorOfCrackAppearance == 0d)
            {
                TraceLogger?.AddMessage($"Section is cracked in start force combination, PsiS = 1.0");
                return 1d;
            }
            var strainTupleInCacking = CalcStrainMatrix(crackResult.TupleOfCrackAppearance as ForceTuple, ndms);
            var stressInCracking = stressLogic.GetStress(TupleConverter.ConvertToLoaderStrainMatrix(strainTupleInCacking), rebarNdm);
            TraceLogger?.AddMessage($"Stress in rebar immediately after cracing Sigma,scrc = {stressInCracking}(Pa)");
            rebarStressActual = stressLogic.GetStress(TupleConverter.ConvertToLoaderStrainMatrix(strainTupleActual), rebarNdm);
            TraceLogger?.AddMessage($"Actual stress in rebar Sigma,s = {rebarStressActual}(Pa)");
            var stressRatio = stressInCracking / rebarStressActual;
            var logic = new ExpSofteningLogic()
            {
                ForceRatio = stressRatio,
                PsiSMin = minimumPsiSFactor,
                PowerFactor = 1d,
                BettaFactor = 0.8d,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            double psiS = logic.GetSofteningFactor();
            TraceLogger?.AddMessage($"PsiS = {psiS}");
            return psiS;
        }

        private CrackForceResult calculateCrackTuples(ForceTuple forceTuple, IEnumerable<INdm> ndms)
        {
            var sectionCrackedLogic = new SectionCrackedLogic()
            {
                NdmCollection = ndms,
                CheckedNdmCollection = new List<INdm>() { concreteNdm },
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(100)
            };
            var crackedLogis = new CrackedLogic(sectionCrackedLogic)
            {
                StartTuple = new ForceTuple(),
                EndTuple = forceTuple,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(100)
            };
            var calculator = new CrackForceCalculator(crackedLogis)
            {
                NdmCollection = ndms,
                EndTuple = forceTuple,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(150)
            };
            calculator.Run();
            return calculator.Result as CrackForceResult;
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
                TraceLogger?.AddMessage(LoggerStrings.CalculationError + $": {forceResult.Description}", TraceLogStatuses.Error);
                throw new StructureHelperException(ErrorStrings.CalculationError);
            }
            var strain = TupleConverter.ConvertToStrainTuple(forceResult.LoaderResults.StrainMatrix);
            return strain;
        }
    }
}
