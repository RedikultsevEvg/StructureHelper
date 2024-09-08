using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Projects
{
    public class FileVersion : IFileVersion
    {
        public Guid Id { get; }
        public int VersionNumber { get; set; }
        public int SubVersionNumber { get; set; }
        public FileVersion(Guid id)
        {
            Id = id;
        }
        public FileVersion() : this(Guid.NewGuid())
        {
            
        }
    }
}
