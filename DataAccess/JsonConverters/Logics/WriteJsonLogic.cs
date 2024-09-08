using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.JsonConverters
{
    public class WriteJsonLogic<T> : IWriteJsonLogic<T>
    {
        private IShouldIgnorePropertyLogic shouldIgnorePropertyLogic;
        private IGetPropertyNameLogic getPropertyNameLogic;
        private IGetIdFromObjectLogic getIdFromObjectLogic;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public WriteJsonLogic(IShouldIgnorePropertyLogic shouldIgnorePropertyLogic,
            IGetPropertyNameLogic getPropertyNameLogic,
            IGetIdFromObjectLogic getIdFromObjectLogic)
        {
            this.shouldIgnorePropertyLogic = shouldIgnorePropertyLogic;
            this.getPropertyNameLogic = getPropertyNameLogic;
            this.getIdFromObjectLogic = getIdFromObjectLogic;
        }
        public WriteJsonLogic()
            : this(new ShouldIgnorePropertyLogic(),
            new GetPropertyNameLogic(),
            new GetIdFromObjectLogic())
        {

        }
        public void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            TraceLogger?.AddMessage($"Serializing {typeof(T).Name} (ID: {getIdFromObjectLogic.GetId(value)})");
            shouldIgnorePropertyLogic.TraceLogger = getPropertyNameLogic.TraceLogger = getIdFromObjectLogic.TraceLogger = TraceLogger;

            // Use JsonSerializer's default behavior to handle attributes like [JsonIgnore] and [JsonProperty]
            var jo = new JObject();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!shouldIgnorePropertyLogic.ShouldIgnoreProperty(prop))
                {
                    string propertyName = getPropertyNameLogic.GetPropertyName(prop);
                    var propValue = prop.GetValue(value);
                    jo.Add(propertyName, JToken.FromObject(propValue, serializer));
                }
            }
            jo.WriteTo(writer);
        }
    }
}
