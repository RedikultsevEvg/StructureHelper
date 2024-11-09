using DataAccess.DTOs;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace DataAccess.Infrastructures
{
    public interface IGetJsonDataByRootObjectLogic : ILogic
    {
        IRootObjectDTO RootObject { get; set; }

        string GetJsonData();
    }
}