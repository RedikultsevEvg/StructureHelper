using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Codes
{
    public interface ICodeEntity : ISaveable
    {
        NatSystems NatSystem { get; }
        string Name { get; set; }
        string FullName { get; set; }

    }
}
