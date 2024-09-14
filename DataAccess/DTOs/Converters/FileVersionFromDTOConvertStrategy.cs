using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;

namespace DataAccess.DTOs
{
    public class FileVersionFromDTOConvertStrategy : IConvertStrategy<FileVersion, FileVersionDTO>
    {
        private IUpdateStrategy<IFileVersion> updateStrategy;

        public IShiftTraceLogger TraceLogger { get; set; }
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public FileVersionFromDTOConvertStrategy(IUpdateStrategy<IFileVersion> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }
        public FileVersionFromDTOConvertStrategy() : this(new FileVersionUpdateStrategy())
        {
            
        }

        public FileVersion Convert(FileVersionDTO source)
        {
            FileVersion fileVersion = new(source.Id);
            updateStrategy.Update(fileVersion, source);
            return fileVersion;
        }
    }
}
