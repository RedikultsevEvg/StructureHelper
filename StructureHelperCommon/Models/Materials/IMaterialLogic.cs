using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Materials
{
    public interface IMaterialLogic : ISaveable
    {
        string Name { get; set; }
        IMaterialLogicOptions Options { get; set; }
        IMaterial GetLoaderMaterial();
        MaterialTypes MaterialType { get; set; }
        DiagramType DiagramType { get; set; }
    }
}
