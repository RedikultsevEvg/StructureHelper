using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Projects;

namespace DataAccess.Infrastructures
{
    public interface IFileOpenLogic : ILogic
    {
        OpenProjectResult OpenFile();
    }
}