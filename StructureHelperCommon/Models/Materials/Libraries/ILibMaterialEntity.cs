using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Codes;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface ILibMaterialEntity : ISaveable
    {
        CodeTypes CodeType { get; }
        ICodeEntity Code { get; set; }
        string Name { get; }
        /// <summary>
        /// Strength of material, Pa
        /// </summary>
        double MainStrength { get; }
    }
}
