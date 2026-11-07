using LD = LoaderCalculator.Data.Materials.DiagramTemplates;

namespace StructureHelperCommon.Models.Materials
{
    internal class CustomConcreteTensionOptionToLoaderConvertStrategy : ICustomConcreteTensionOptionToLoaderConvertStrategy
    {
        public bool WorkInTension { get; set; }
        public double StrengthFactor { get; set; } = 1d;
        public LD.CustomConcreteTensionOptions Convert(ICustomConcreteTensionOptions source)
        {
            LD.CustomConcreteTensionOptions options = new ()
            {
                Strength = source.Strength,
                StrengthFactor = StrengthFactor,
                UltimateStrain = source.UltimateStrain,
                LimitStrain = source.LimitStrain,
                CrackStrain = source.UltimateStrain,
                WorkInTension = WorkInTension,
            };
            return options;
        }
    }
}
