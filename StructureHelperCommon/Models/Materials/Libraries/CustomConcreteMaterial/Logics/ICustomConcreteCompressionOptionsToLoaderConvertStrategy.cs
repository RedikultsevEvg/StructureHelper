using StructureHelperCommon.Infrastructures.Interfaces;
using LD = LoaderCalculator.Data.Materials.DiagramTemplates;

namespace StructureHelperCommon.Models.Materials
{
    internal interface ICustomConcreteCompressionOptionsToLoaderConvertStrategy : IObjectConvertStrategy<LD.CustomConcreteCompressionOptions, ICustomConcreteCompressionOptions>
    {
        double StrengthFactor { get; set; }
    }
}