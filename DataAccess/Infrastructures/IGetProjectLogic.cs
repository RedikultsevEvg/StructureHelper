using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public interface IGetProjectLogic : ILogic
    {
        string FileName { get; set; }
        IShiftTraceLogger TraceLogger { get; set; }
        OpenProjectResult GetProject();
    }
}
