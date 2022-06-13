using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialResistanceCalc
{
    public class MaterialDefinition
    {
        public string MaterialClass { get; set; }
        public double YoungModulus { get; set; }
        public double CompressiveStrength { get; set; }
        public double TensileStrength { get; set; }
        public double MaterialCoefInCompress { get; set; }
        public double MaterialCoefInTension { get; set; }

        public MaterialDefinition(string materialClass, double youngModulus, double compressiveStrengthCoef, double tensileStrengthCoef, double materialCoefInCompress, double materialCoefInTension)
        {
            MaterialClass = materialClass;
            YoungModulus = youngModulus;
            CompressiveStrength = compressiveStrengthCoef;
            TensileStrength = tensileStrengthCoef;
            MaterialCoefInCompress = materialCoefInCompress;
            MaterialCoefInTension = materialCoefInTension;
        }
    }
}
