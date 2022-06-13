using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialResistanceCalc
{
    public class ConcreteDefinition : MaterialDefinition
    {
        public ConcreteDefinition(string materialClass, double youngModulus, double compressiveStrengthCoef, double tensileStrengthCoef, double materialCoefInCompress, double materialCoefInTension)
            : base(materialClass, youngModulus, compressiveStrengthCoef, tensileStrengthCoef, materialCoefInCompress, materialCoefInTension)
        {
            CompressiveStrength = compressiveStrengthCoef * Math.Pow(10, 6);
            TensileStrength = tensileStrengthCoef * Math.Sqrt(CompressiveStrength);
        }
    }
}
