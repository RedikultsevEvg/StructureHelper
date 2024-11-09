using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Projects;

namespace DataAccess.Infrastructures
{
    public interface ISaveProjectToFileLogic : ILogic
    {
        IProject Project { get; set; }

        void SaveProject();
    }
}