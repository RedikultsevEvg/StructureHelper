using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public class ProjectAccessLogic : IProjectAccessLogic
    {
        private IFileOpenLogic openLogic;
        private IFileSaveLogic saveLogic;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public ProjectAccessLogic(IFileOpenLogic openLogic, IFileSaveLogic saveLogic)
        {
            this.openLogic = openLogic;
            this.saveLogic = saveLogic;
        }

        public ProjectAccessLogic() : this(new FileOpenLogic(), new FileSaveLogic())
        {

        }

        public OpenProjectResult OpenProject()
        {
            openLogic.TraceLogger = TraceLogger;
            return openLogic.OpenFile();
        }

        public void SaveProject(IProject project)
        {
            saveLogic.TraceLogger = TraceLogger;
            saveLogic.SaveFile(project);
        }

        public void SaveProjectAs(IProject project)
        {
            saveLogic.TraceLogger = TraceLogger;
            saveLogic.SaveFileAs(project);
        }
    }
}
