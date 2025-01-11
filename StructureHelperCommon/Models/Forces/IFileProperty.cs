using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Forces
{
    public interface IFileProperty : ISaveable
    {
        string FilePath { get; set; }
    }
}