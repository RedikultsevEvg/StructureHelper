using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    internal enum CrackWidthLogicType
    {
        SP63
    }
    internal static class CrackWidthLogicInputDataFactory
    {
        static IStressLogic stressLogic => new StressLogic();
        public static ICrackWidthLogicInputData GetCrackWidthLogicInputData(CrackWidthLogicType logicType, ICrackWidthSimpleCalculatorInputData inputData)
        {
            if (logicType == CrackWidthLogicType.SP63)
            {
                CrackWidthLogicInputDataSP63 data = new();
                ProcessBaseProps(inputData, data);
                if (inputData.CalcTerm == CalcTerms.LongTerm) { data.TermFactor = 1.4d; }
                else { data.TermFactor = 1d; }
                data.PsiSFactor = inputData.PsiSFactor;
                data.StressStateFactor = inputData.StressState is SectionStressStates.Tension ? 1.2d : 1.0d;
                data.BondFactor = 0.5;
                return data;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
            }

        }

        private static void ProcessBaseProps(ICrackWidthSimpleCalculatorInputData inputData, ICrackWidthLogicInputData data)
        {
            var strainMatrix = TupleConverter.ConvertToLoaderStrainMatrix(inputData.StrainTuple);
            var triangulationOptions = new TriangulationOptions { LimiteState = inputData.LimitState, CalcTerm = inputData.CalcTerm };
            var ndms = inputData.RebarPrimitive.GetNdms(triangulationOptions).ToArray();
            var concreteNdm = ndms[0];
            var rebarNdm = ndms[1];
            data.ConcreteStrain = concreteNdm.Prestrain;// stressLogic.GetTotalStrain(strainMatrix, concreteNdm) - stressLogic.GetTotalStrainWithPresrain(strainMatrix, concreteNdm);
            data.RebarStrain = stressLogic.GetTotalStrainWithPresrain(strainMatrix, rebarNdm);
            data.Length = inputData.Length;
        }
    }
}
