using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Logics;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Analyses.RC;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.ResultViewers
{
    public static class ShowAnchorageResult
    {
        private static IStressLogic stressLogic => new StressLogic();
        public static void ShowAnchorageField (IStrainMatrix strainMatrix, LimitStates limitState, CalcTerms calcTerm, IEnumerable<PrimitiveBase> primitives)
        {
            foreach (var item in primitives)
            {
                if (item is ReinforcementViewPrimitive)
                {
                    var primitive = item as ReinforcementViewPrimitive;
                    var ndmPrimitive = primitive.GetNdmPrimitive() as ReinforcementPrimitive;
                    var inputData = new AnchorageInputData();
                    inputData.ConcreteStrength = GetConcreteStrength(limitState, calcTerm, ndmPrimitive);
                    inputData.ReinforcementStrength = GetReinforcementStrength(limitState, calcTerm, ndmPrimitive);
                    inputData.CrossSectionArea = ndmPrimitive.Area;
                    var diameter = Math.Sqrt(ndmPrimitive.Area / Math.PI) * 2d;
                    inputData.CrossSectionPerimeter = Math.PI * diameter;
                    var material = ndmPrimitive.HeadMaterial.GetLoaderMaterial(limitState, calcTerm);
                    var ndm = ndmPrimitive.GetNdms(material).Single();
                    inputData.ReinforcementStress = stressLogic.GetStress(strainMatrix, ndm);
                    inputData.LappedCountRate = 0.5d;
                }
            }
        }
        private static double GetConcreteStrength(LimitStates limitState, CalcTerms calcTerm, ReinforcementPrimitive primitive)
        {
            if (primitive.HostPrimitive is not null)
            {
                var host = primitive.HostPrimitive;
                var hostMaterial = host.HeadMaterial.HelperMaterial;
                if (hostMaterial is IConcreteLibMaterial)
                {
                    var concreteMaterial = hostMaterial as IConcreteLibMaterial;
                    var concreteStrength = concreteMaterial.GetStrength(limitState, calcTerm).Tensile;
                    return concreteStrength;
                }
            }
            throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": host's material is incorrect or null");
        }

        private static double GetReinforcementStrength(LimitStates limitState, CalcTerms calcTerm, ReinforcementPrimitive primitive)
        {
            if (primitive.HeadMaterial.HelperMaterial is IReinforcementLibMaterial)
            {
                var material = primitive.HeadMaterial.HelperMaterial as IReinforcementLibMaterial;
                var strength = material.GetStrength(limitState, calcTerm).Tensile;
                return strength;
            }
            throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": host's material is incorrect or null");
        }
    }
}
