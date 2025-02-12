using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials.Logics
{
    internal class FunctionMaterialLogic : IFunctionMaterialLogic
    {
        private List<double> parameters;
        public IMaterial GetLoaderMaterial(IFunctionMaterial functionMaterial, LimitStates limitState, CalcTerms calcTerm, double factor = 1d)
        {
            IMaterial material = new Material();
            /*material.InitModulus = functionMaterial.Modulus;
            IFactorLogic factorLogic = new FactorLogic(functionMaterial.SafetyFactors);
            var factors = factorLogic.GetTotalFactor(limitState, calcTerm);
            parameters = new List<double>()
            {
                functionMaterial.Modulus,
                functionMaterial.CompressiveStrength * factors.Compressive * factor,
                functionMaterial.TensileStrength * factors.Tensile * factor
            };
            material.DiagramParameters = parameters;
            material.Diagram = GetStressByStrain;*/
            return material;
        }
        private double GetStressByStrain(IEnumerable<double> parameters1, double strain)
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
