using LD = LoaderCalculator.Data.Materials.DiagramTemplates;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Materials
{
    internal interface ICustomConcreteOptionsToLoaderConvertStrategy : IObjectConvertStrategy<LD.CustomConcreteOptions, ICustomConcreteModel>
    {
        bool WorkInTension { get; set; }
        double CompressionStrengthFactor { get; set; }
        double TensionStrengthFactor { get; set; }

    }
}