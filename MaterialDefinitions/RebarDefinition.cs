using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialResistanceCalc
{
    public class RebarDefinition : MaterialDefinition
    {
        public RebarDefinition(string materialClass, double youngModulus, double compressiveStrengthCoef, double tensileStrengthCoef, double materialCoefInCompress, double materialCoefInTension) : base(materialClass, youngModulus, compressiveStrengthCoef, tensileStrengthCoef, materialCoefInCompress, materialCoefInTension)
        {
            YoungModulus = youngModulus * Math.Pow(10, 11);
            CompressiveStrength = compressiveStrengthCoef * Math.Pow(10, 6);
            TensileStrength = tensileStrengthCoef * Math.Pow(10, 6);
        }
    }
}
