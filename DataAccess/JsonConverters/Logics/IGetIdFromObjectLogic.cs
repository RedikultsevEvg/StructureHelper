using StructureHelperCommon.Infrastructures.Interfaces;

namespace DataAccess.JsonConverters
{
    /// <summary>
    /// Logic to get the ID for logging purposes, assumes all classes have an 'Id' property of type Guid.
    /// </summary>
    public interface IGetIdFromObjectLogic : ILogic
    {
        Guid GetId(object obj);
    }
}