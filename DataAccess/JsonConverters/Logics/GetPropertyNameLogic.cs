using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.JsonConverters
{
    public class GetPropertyNameLogic : IGetPropertyNameLogic
    {
        public IShiftTraceLogger? TraceLogger { get; set; }

        // Helper method to get the property name, considering [JsonProperty] and [JsonPropertyName] attributes
        public string GetPropertyName(PropertyInfo prop)
        {
            // Check for [JsonProperty] attribute (for Newtonsoft.Json)
            var jsonPropertyAttribute = prop.GetCustomAttribute<JsonPropertyAttribute>();
            if (jsonPropertyAttribute != null)
            {
                return jsonPropertyAttribute.PropertyName;
            }

            // Check for [JsonPropertyName] attribute (for System.Text.Json compatibility)
            var jsonPropertyNameAttribute = prop.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>();
            if (jsonPropertyNameAttribute != null)
            {
                return jsonPropertyNameAttribute.Name;
            }

            // Default to the property name if no attributes are found
            return prop.Name;
        }
    }
}
