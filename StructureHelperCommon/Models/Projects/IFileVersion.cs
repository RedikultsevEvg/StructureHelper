using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Projects
{
    public interface IFileVersion : ISaveable
    {
        int VersionNumber { get; set; }
        int SubVersionNumber { get; set; }
    }
}
