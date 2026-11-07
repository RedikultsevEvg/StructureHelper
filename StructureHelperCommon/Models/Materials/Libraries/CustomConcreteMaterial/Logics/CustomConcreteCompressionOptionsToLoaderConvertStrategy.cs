using StructureHelperCommon.Services;
using LD = LoaderCalculator.Data.Materials.DiagramTemplates;

namespace StructureHelperCommon.Models.Materials
{
    internal class CustomConcreteCompressionOptionsToLoaderConvertStrategy : ICustomConcreteCompressionOptionsToLoaderConvertStrategy
    {
        public double StrengthFactor { get; set; } = 1d;
        public LD.CustomConcreteCompressionOptions Convert(ICustomConcreteCompressionOptions source)
        {
            CheckObject.ThrowIfNull(source, "source");
            LD.CustomConcreteCompressionOptions options = new()
            {
                PeakStress = source.PeakStress,
                StrengthFactor = StrengthFactor,
                PeakStrain = source.PeakStrain,
                LimitStrain = source.LimitStrain,
                DescendingScaleFactor = source.DescendingScaleFactor
            };
            return options;
        }
    }
}
