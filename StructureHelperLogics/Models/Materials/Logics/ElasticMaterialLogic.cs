using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    internal class ElasticMaterialLogic : IElasticMaterialLogic
    {
        public IMaterial GetLoaderMaterial(IElasticMaterial elasticMaterial, LimitStates limitState, CalcTerms calcTerm, double factor = 1d)
        { 
            IMaterial material = new Material();
            material.InitModulus = elasticMaterial.Modulus;
            IFactorLogic factorLogic = new FactorLogic(elasticMaterial.SafetyFactors);
            var factors = factorLogic.GetTotalFactor(limitState, calcTerm);
            IEnumerable<double> parameters = new List<double>()
            {
                elasticMaterial.Modulus,
                elasticMaterial.CompressiveStrength * factors.Compressive * factor,
                elasticMaterial.TensileStrength * factors.Tensile * factor
            };
        material.DiagramParameters = parameters;
        material.Diagram = GetStressByStrain;
        return material;
        }

    private double GetStressByStrain(IEnumerable<double> parameters, double strain)
    {
        double modulus = parameters.First();
        double stress = modulus * strain;
        double compressiveStrength = (-1d) * parameters.ElementAt(1);
        double tensileStrength = parameters.ElementAt(2);
        if (stress > tensileStrength || stress < compressiveStrength) { return 0d; }
        else { return stress; }
    }
}
}
