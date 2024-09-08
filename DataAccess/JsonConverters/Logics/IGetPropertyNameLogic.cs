using StructureHelperCommon.Infrastructures.Interfaces;
using System.Reflection;

namespace DataAccess.JsonConverters
{
    /// <summary>
    /// Helper logic to get the property name, considering [JsonProperty] and [JsonPropertyName] attributes
    /// </summary>
    public interface IGetPropertyNameLogic : ILogic
    {
        string GetPropertyName(PropertyInfo prop);
    }
}