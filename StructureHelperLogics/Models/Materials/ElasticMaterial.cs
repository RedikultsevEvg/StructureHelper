using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class ElasticMaterial : IElasticMaterial
    {
        public double Modulus { get; set; }
        public double CompressiveStrength { get; set; }
        public double TensileStrength { get; set; }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            IMaterial material = new Material();
            material.InitModulus = Modulus;
            IEnumerable<double> parameters = new List<double>() { Modulus, CompressiveStrength, TensileStrength};
            material.DiagramParameters = parameters;
            material.Diagram = GetStress;
            return material;
        }

        private double GetStress (IEnumerable<double> parameters, double strain)
        {
            double modulus = parameters.First();
            double stress = modulus * strain;
            double compressiveStrength = (-1d) * parameters.ElementAt(1);
            double tensileStrength = parameters.ElementAt(2);
            if (stress > tensileStrength || stress < compressiveStrength) { return 0d; }
            else { return stress; }
        }

        public object Clone()
        {
            return new ElasticMaterial() { Modulus = Modulus, CompressiveStrength = CompressiveStrength, TensileStrength = TensileStrength };
        }
    }
}
