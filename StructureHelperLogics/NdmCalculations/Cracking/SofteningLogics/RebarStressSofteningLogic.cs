using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarStressSofteningLogic : ICrackSofteningLogic
    {
        private IStressLogic stressLogic;
        private RebarStressResult afterCrackingRebarResult;
        private RebarStressResult actualRebarResult;


        private INdm concreteNdm;
        private INdm rebarNdm;

        private double rebarStrainActual;
        private double concreteStrainActual;
        private double rebarStressActual;

        public double MinValueOfFactor { get; set; } = 0.2d;
        public RebarPrimitive RebarPrimitive { get; set; }
        public RebarCrackInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetSofteningFactor()
        {
            GetNdms();
            return GetPsiSFactor(InputData.ForceTuple, InputData.CrackableNdmCollection, InputData.CrackedNdmCollection);
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

        private double GetPsiSFactor(ForceTuple forceTuple, IEnumerable<INdm> crackableNndms, IEnumerable<INdm> crackedNndms)
        {

            var crackResult = calculateCrackTuples(forceTuple, crackableNndms);
            if (crackResult.IsValid == false)
            {
                string errorString = LoggerStrings.CalculationError + crackResult.Description;
                TraceLogger?.AddMessage($"Rebar name: {RebarPrimitive.Name}\n" + errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }


            actualRebarResult = GetRebarStressResult(forceTuple);
            rebarStrainActual = actualRebarResult.RebarStrain;
            rebarStressActual = actualRebarResult.RebarStress;
            TraceLogger?.AddMessage($"Actual strain of rebar EpsilonS = {rebarStrainActual}(dimensionless)");
            concreteStrainActual = concreteNdm.Prestrain;
            //concreteStrainActual = stressLogic.GetTotalStrain(TupleConverter.ConvertToLoaderStrainMatrix(strainTupleActual), concreteNdm);
            TraceLogger?.AddMessage($"Actual strain of concrete on the axis of rebar EpsilonC = {concreteStrainActual}(dimensionless)");
            if (crackResult.IsSectionCracked == false)
            {
                TraceLogger?.AddMessage($"Section is not cracked PsiS = {MinValueOfFactor}");
                return MinValueOfFactor;
            }
            if (crackResult.FactorOfCrackAppearance == 0d)
            {
                TraceLogger?.AddMessage($"Section is cracked in start force combination, PsiS = 1.0");
                return 1d;
            }
            afterCrackingRebarResult = GetRebarStressResult(crackResult.TupleOfCrackAppearance as ForceTuple);

            var stressInCracking = afterCrackingRebarResult.RebarStress;
            TraceLogger?.AddMessage($"Stress in rebar immediately after cracking Sigma,scrc = {stressInCracking}(Pa)");
            TraceLogger?.AddMessage($"Actual stress in rebar Sigma,s = {rebarStressActual}(Pa)");
            double psiS = GetExponentialSofteningFactor(stressInCracking);
            TraceLogger?.AddMessage($"PsiS = {psiS}");
            //return 0.94d;
            return psiS;
        }

        private double GetExponentialSofteningFactor(double stressInCracking)
        {
            var stressRatio = stressInCracking / rebarStressActual;
            var logic = new ExpSofteningLogic()
            {
                ForceRatio = stressRatio,
                PsiSMin = MinValueOfFactor,
                PowerFactor = 1d,
                BettaFactor = 0.8d,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            double psiS = logic.GetSofteningFactor();
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
                Accuracy = new Accuracy()
                {
                    IterationAccuracy = 0.01d,
                    MaxIterationCount = 1000
                },
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(150)
            };
            calculator.Run();
            return calculator.Result as CrackForceResult;
        }

        private RebarStressResult GetRebarStressResult(ForceTuple forceTuple)
        {
            var calculator = new RebarStressCalculator()
            {
                ForceTuple = forceTuple,
                NdmCollection = InputData.CrackedNdmCollection,
                RebarPrimitive = RebarPrimitive
            };
            calculator.Run();
            var result = calculator.Result as RebarStressResult;
            if (result.IsValid == false)
            {
                string errorString = LoggerStrings.CalculationError + result.Description;
                TraceLogger?.AddMessage($"Rebar name: {RebarPrimitive.Name}\n" + errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            return result;
        }
    }
}
