using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.RC
{
    public static class InputDataFactory
    {
        private static IStressLogic stressLogic => new StressLogic();
        public static IAnchorageInputData GetInputData(RebarPrimitive ndmPrimitive, IStrainMatrix strainMatrix, LimitStates limitState, CalcTerms calcTerm, double lappedCountRate)
        {
            var inputData = new AnchorageInputData();
            inputData.ConcreteStrength = GetConcreteStrength(limitState, calcTerm, ndmPrimitive);
            inputData.ReinforcementStrength = GetReinforcementStrength(limitState, calcTerm, ndmPrimitive);
            inputData.FactorEta1 = 2.5d;
            inputData.CrossSectionArea = ndmPrimitive.Area;
            var diameter = Math.Sqrt(ndmPrimitive.Area / Math.PI) * 2d;
            inputData.CrossSectionPerimeter = Math.PI * diameter;
            if (ndmPrimitive.NdmElement.HeadMaterial is null)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": main material is incorrect or null");
            }
            var triangulationOptions = new TriangulationOptions() { LimiteState = limitState, CalcTerm = calcTerm };
            var ndms = ndmPrimitive.GetNdms(triangulationOptions);
            var ndm = ndms.Where(x => x is RebarNdm)
                .Single();
            if (strainMatrix is not null)
            {
                inputData.ReinforcementStress = stressLogic.GetStress(strainMatrix, ndm);
            }
            else
            {
                inputData.ReinforcementStress = inputData.ReinforcementStrength;
            }
            inputData.IsPrestressed = ndm.PrestrainLogic.GetByType(PrestrainTypes.Prestrain).Sum(x => x.PrestrainValue) > 0.0005d ? true : false; 
            inputData.LappedCountRate = lappedCountRate;
            return inputData;
        }

        private static double GetConcreteStrength(LimitStates limitState, CalcTerms calcTerm, RebarPrimitive primitive)
        {
            if (primitive.HostPrimitive is not null)
            {
                var host = primitive.HostPrimitive;
                var hostMaterial = host
                    .NdmElement
                    .HeadMaterial
                    .HelperMaterial;
                if (hostMaterial is IConcreteLibMaterial)
                {
                    var concreteMaterial = hostMaterial as IConcreteLibMaterial;
                    var concreteStrength = concreteMaterial.GetStrength(limitState, calcTerm).Tensile;
                    return concreteStrength;
                }
            }
            throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": host material is incorrect or null");
        }

        private static double GetReinforcementStrength(LimitStates limitState, CalcTerms calcTerm, RebarPrimitive primitive)
        {
            if (primitive.NdmElement.HeadMaterial.HelperMaterial is IReinforcementLibMaterial)
            {
                var material = primitive.NdmElement.HeadMaterial.HelperMaterial as IReinforcementLibMaterial;
                var strength = material.GetStrength(limitState, calcTerm).Tensile;
                return strength;
            }
            throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": host's material is incorrect or null");
        }
    }
}
