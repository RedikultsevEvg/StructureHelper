using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface ILibMaterialEntity
    {
        CodeTypes CodeType { get; }
        string Name { get; }
        double MainStrength { get; }
    }
}
