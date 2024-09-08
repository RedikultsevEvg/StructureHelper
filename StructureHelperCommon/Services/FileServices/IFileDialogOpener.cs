namespace StructureHelperCommon.Services.FileServices
{
    public interface IFileDialogOpener
    {
        OpenFileInputData? InputData { get; }

        OpenFileResult OpenFile();
    }
}