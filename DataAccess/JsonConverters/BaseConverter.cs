using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StructureHelperCommon.Models;
using System;
using System.Reflection;

namespace DataAccess.JsonConverters
{


    public abstract class BaseConverter<T> : JsonConverter<T>
    {
        private IShiftTraceLogger traceLogger;

        protected BaseConverter(IShiftTraceLogger logger)
        {
            traceLogger = logger;
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            traceLogger.AddMessage($"Serializing {typeof(T).Name} (ID: {GetId(value)})");

            // Use JsonSerializer's default behavior to handle attributes like [JsonIgnore] and [JsonProperty]
            var jo = new JObject();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!ShouldIgnoreProperty(prop))
                {
                    string propertyName = GetPropertyName(prop);
                    var propValue = prop.GetValue(value);
                    jo.Add(propertyName, JToken.FromObject(propValue, serializer));
                }
            }
            jo.WriteTo(writer);
        }

        // Helper method to check if a property should be ignored
        private bool ShouldIgnoreProperty(PropertyInfo prop)
        {
            // Check for [JsonIgnore] attribute
            var jsonIgnoreAttribute = prop.GetCustomAttribute<JsonIgnoreAttribute>();
            return jsonIgnoreAttribute != null;
        }

        // Helper method to get the property name, considering [JsonProperty] and [JsonPropertyName] attributes
        private string GetPropertyName(PropertyInfo prop)
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

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            traceLogger.AddMessage($"Deserializing {typeof(T).Name}");
            // Use JsonSerializer's default behavior to handle attributes during deserialization
            JObject jo = JObject.Load(reader);
            T obj = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (!ShouldIgnoreProperty(prop) && jo.TryGetValue(GetPropertyName(prop), out JToken value))
                {
                    var propValue = value.ToObject(prop.PropertyType, serializer);
                    prop.SetValue(obj, propValue);
                }
            }

            traceLogger.AddMessage($"Deserialized {typeof(T).Name} (ID: {GetId(obj)})");
            return obj;
        }

        // Method to get the ID for logging purposes, assumes all classes have an 'Id' property of type Guid.
        private Guid GetId(object obj)
        {
            var idProp = obj.GetType().GetProperty("Id");
            return idProp != null ? (Guid)idProp.GetValue(obj) : Guid.Empty;
        }
    }

}
