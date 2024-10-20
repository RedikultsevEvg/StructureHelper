using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public interface IRootObjectDTO
    {
        FileVersionDTO FileVersion { get; set; }
        ProjectDTO Project { get; set; }
    }
}
