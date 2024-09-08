using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class FileVersionDTOConvertStrategy : IConvertStrategy<FileVersion, FileVersionDTO>
    {
        private IUpdateStrategy<IFileVersion> updateStrategy;

        public IShiftTraceLogger TraceLogger { get; set; }

        public FileVersionDTOConvertStrategy(IUpdateStrategy<IFileVersion> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }
        public FileVersionDTOConvertStrategy() : this(new FileVersionUpdateStrategy())
        {
            
        }

        public FileVersionDTO ConvertTo(FileVersion source)
        {
            FileVersionDTO fileVersion = new(source.Id);
            updateStrategy.Update(fileVersion, source);
            return fileVersion;
        }

        public FileVersion ConvertFrom(FileVersionDTO source)
        {
            FileVersion fileVersion = new(source.Id);
            updateStrategy.Update(fileVersion, source);
            return fileVersion;
        }
    }
}
