using DataAccess.DTOs;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.JsonConverters
{
    internal class ProjectDTOJsonConverter : BaseJsonConverter<ProjectDTO>
    {
        public ProjectDTOJsonConverter(IShiftTraceLogger logger) : base(logger)
        {
        }
    }
}
