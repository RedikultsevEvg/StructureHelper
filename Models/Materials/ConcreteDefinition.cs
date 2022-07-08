using System;

namespace StructureHelper.Models.Materials
{
    public class ConcreteDefinition : MaterialDefinitionBase
    {
        public ConcreteDefinition(string materialClass, double youngModulus, double compressiveStrengthCoef, double tensileStrengthCoef, double materialCoefInCompress, double materialCoefInTension)
            : base(materialClass, youngModulus, compressiveStrengthCoef, tensileStrengthCoef, materialCoefInCompress, materialCoefInTension)
        {
            CompressiveStrength = compressiveStrengthCoef/* * Math.Pow(10, 6)*/;
            TensileStrength = Math.Round(620 * Math.Sqrt(CompressiveStrength), 2);
            YoungModulus = Math.Round(4.7 * Math.Sqrt(DesignCompressiveStrength), 2);
        }
    }
}
