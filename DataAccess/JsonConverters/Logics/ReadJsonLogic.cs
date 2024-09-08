using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.JsonConverters
{
    public class ReadJsonLogic<T> : IReadJsonLogic<T>
    {
        private IShouldIgnorePropertyLogic shouldIgnorePropertyLogic;
        private IGetPropertyNameLogic getPropertyNameLogic;
        private IGetIdFromObjectLogic getIdFromObjectLogic;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public ReadJsonLogic(IShouldIgnorePropertyLogic shouldIgnorePropertyLogic,
            IGetPropertyNameLogic getPropertyNameLogic,
            IGetIdFromObjectLogic getIdFromObjectLogic)
        {
            this.shouldIgnorePropertyLogic = shouldIgnorePropertyLogic;
            this.getPropertyNameLogic = getPropertyNameLogic;
            this.getIdFromObjectLogic = getIdFromObjectLogic;
        }
        public ReadJsonLogic()
            : this(new ShouldIgnorePropertyLogic(),
            new GetPropertyNameLogic(),
            new GetIdFromObjectLogic())
        {

        }

        public T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            TraceLogger?.AddMessage($"Deserializing {typeof(T).Name}");
            shouldIgnorePropertyLogic.TraceLogger = getPropertyNameLogic.TraceLogger = getIdFromObjectLogic.TraceLogger = TraceLogger;
            // Use JsonSerializer's default behavior to handle attributes during deserialization
            JObject jo = JObject.Load(reader);
            T obj = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (! shouldIgnorePropertyLogic.ShouldIgnoreProperty(prop) && jo.TryGetValue(getPropertyNameLogic.GetPropertyName(prop), out JToken value))
                {
                    var propValue = value.ToObject(prop.PropertyType, serializer);
                    prop.SetValue(obj, propValue);
                }
            }

            TraceLogger?.AddMessage($"Deserialized {typeof(T).Name} (ID: {getIdFromObjectLogic.GetId(obj)})");
            return obj;
        }
    }
}
