using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;

namespace DataAccess.DTOs
{
    public class FileVersionToDTOConvertStrategy : IConvertStrategy<FileVersionDTO, IFileVersion>
    {
        private IUpdateStrategy<IFileVersion> updateStrategy;

        public IShiftTraceLogger TraceLogger { get; set; }
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }

        public FileVersionToDTOConvertStrategy(IUpdateStrategy<IFileVersion> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }
        public FileVersionToDTOConvertStrategy() : this(new FileVersionUpdateStrategy())
        {
            
        }

        public FileVersionDTO Convert(IFileVersion source)
        {
            FileVersionDTO fileVersion = new(source.Id);
            updateStrategy.Update(fileVersion, source);
            return fileVersion;
        }
    }
}
