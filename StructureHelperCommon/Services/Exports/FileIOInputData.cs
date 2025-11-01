namespace StructureHelperCommon.Services.Exports
{
    public class FileIOInputData : IFileIOnputData
    {
        public string FileName { get; set; } = "New file";
        public string Filter { get; set; }
        public string Title { get; set; }
    }
}
