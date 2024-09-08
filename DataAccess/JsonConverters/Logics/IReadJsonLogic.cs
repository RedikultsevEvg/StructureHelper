using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace DataAccess.JsonConverters
{
    /// <summary>
    /// Helper logic for JSON converter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadJsonLogic<T> : ILogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer);
    }
}