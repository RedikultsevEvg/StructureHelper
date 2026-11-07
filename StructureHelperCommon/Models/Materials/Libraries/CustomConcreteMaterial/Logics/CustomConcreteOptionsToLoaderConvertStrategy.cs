using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using LD = LoaderCalculator.Data.Materials.DiagramTemplates;

namespace StructureHelperCommon.Models.Materials
{
    internal class CustomConcreteOptionsToLoaderConvertStrategy : ICustomConcreteOptionsToLoaderConvertStrategy
    {
        private ICustomConcreteCompressionOptionsToLoaderConvertStrategy compressionConvertStrategy;
        private ICustomConcreteCompressionOptionsToLoaderConvertStrategy CompressionConvertStrategy => compressionConvertStrategy ??= new CustomConcreteCompressionOptionsToLoaderConvertStrategy();
        private ICustomConcreteTensionOptionToLoaderConvertStrategy tensionConvertStrategy;
        private ICustomConcreteTensionOptionToLoaderConvertStrategy TensionConvertStrategy => tensionConvertStrategy ??= new CustomConcreteTensionOptionToLoaderConvertStrategy();


        public bool WorkInTension { get; set; }
        public double CompressionStrengthFactor { get; set; } = 1d;
        public double TensionStrengthFactor { get; set; }

        public LD.CustomConcreteOptions Convert(ICustomConcreteModel source)
        {
            CheckObject.ThrowIfNull(source);
            CheckObject.ThrowIfNull(source.CompressionOptions);
            CheckObject.ThrowIfNull(source.TensionOptions);
            CompressionConvertStrategy.StrengthFactor = CompressionStrengthFactor;
            TensionConvertStrategy.StrengthFactor = TensionStrengthFactor;
            TensionConvertStrategy.WorkInTension = WorkInTension;
            LD.CustomConcreteOptions options = new()
            {
                YoungModulus = source.YoungModulus,
                CompressionOptions = CompressionConvertStrategy.Convert(source.CompressionOptions),
                TensionOptions = TensionConvertStrategy.Convert(source.TensionOptions)
            };
            return options;
        }
    }
}
