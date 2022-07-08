namespace StructureHelper.Models.Materials
{
    public class RebarDefinition : MaterialDefinitionBase
    {
        public RebarDefinition(string materialClass, double youngModulus, double compressiveStrengthCoef, double tensileStrengthCoef, double materialCoefInCompress, double materialCoefInTension) : base(materialClass, youngModulus, compressiveStrengthCoef, tensileStrengthCoef, materialCoefInCompress, materialCoefInTension)
        {
            YoungModulus = youngModulus/* * Math.Pow(10, 11)*/;
            CompressiveStrength = compressiveStrengthCoef/* * Math.Pow(10, 6)*/;
            TensileStrength = tensileStrengthCoef/* * Math.Pow(10, 6)*/;
        }
    }
}
