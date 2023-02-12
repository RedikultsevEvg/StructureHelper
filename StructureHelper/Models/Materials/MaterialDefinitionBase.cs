using System;

namespace StructureHelper.Models.Materials
{
    public class MaterialDefinitionBase
    {
        public string MaterialClass { get; set; }
        public double YoungModulus { get; set; }
        public double CompressiveStrength { get; set; }
        public double TensileStrength { get; set; }
        public double MaterialCoefInCompress { get; set; }
        public double MaterialCoefInTension { get; set; }
        public double DesignCompressiveStrength { get; set; }
        public double DesingTensileStrength { get; set; }

        public MaterialDefinitionBase(string materialClass, double youngModulus, double compressiveStrengthCoef, double tensileStrengthCoef, double materialCoefInCompress, double materialCoefInTension)
        {
            MaterialClass = materialClass;
            YoungModulus = youngModulus;
            CompressiveStrength = compressiveStrengthCoef;
            TensileStrength = tensileStrengthCoef;
            MaterialCoefInCompress = materialCoefInCompress;
            MaterialCoefInTension = materialCoefInTension;
            DesignCompressiveStrength = compressiveStrengthCoef * Math.Pow(10, 6) * 1 / 1;
            DesingTensileStrength = tensileStrengthCoef * Math.Pow(10, 3) * 1 / materialCoefInTension;
        }
    }
}
