using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StructureHelperLogics.Models.Materials
{
    internal class ElasticMaterialLogic : IElasticMaterialLogic
    {
        private double modulusOfElasticity;
        private double compressiveStrength;
        private double tensileStrength;

        public IMaterial GetLoaderMaterial(IElasticMaterial elasticMaterial, LimitStates limitState, CalcTerms calcTerm, double factor = 1d)
        { 
            IMaterial material = new Material();
            material.InitModulus = elasticMaterial.Modulus;
            IMaterialFactorLogic factorLogic = new MaterialFactorLogic(elasticMaterial.SafetyFactors);
            var factors = factorLogic.GetTotalFactor(limitState, calcTerm);
            modulusOfElasticity = elasticMaterial.Modulus;
            compressiveStrength = (-1d) * elasticMaterial.CompressiveStrength * factors.Compressive * factor;
            tensileStrength = elasticMaterial.TensileStrength * factors.Tensile * factor;
            material.Diagram = GetStressByStrain;
            return material;
        }

    private double GetStressByStrain(double strain)
    {
        double stress = modulusOfElasticity * strain;
        if (stress > tensileStrength || stress < compressiveStrength) { return 0d; }
        else { return stress; }
    }
}
}
