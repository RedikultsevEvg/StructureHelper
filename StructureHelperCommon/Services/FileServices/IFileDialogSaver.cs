using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Services.FileServices
{
    public interface IFileDialogSaver : ILogic
    {
        SaveFileResult SaveFile();
    }
}