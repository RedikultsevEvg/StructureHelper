using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public interface IProjectAccessLogic : ILogic
    {
        OpenProjectResult OpenProject();
        void SaveProject(IProject project);
        void SaveProjectAs(IProject project);
    }
}
