using DataAccess.DTOs;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace DataAccess.Infrastructures
{
    public interface IGetRootObjectLogic : ILogic
    {
        RootObjectDTO? GetRootObject();
    }
}