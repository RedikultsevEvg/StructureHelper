using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace DataAccess.JsonConverters
{
    public interface IWriteJsonLogic<T> : ILogic
    {
        void WriteJson(JsonWriter writer, T value, JsonSerializer serializer);
    }
}