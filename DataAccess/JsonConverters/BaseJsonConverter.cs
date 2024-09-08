using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StructureHelperCommon.Models;
using System;
using System.Reflection;

namespace DataAccess.JsonConverters
{


    public abstract class BaseJsonConverter<T> : JsonConverter<T>
    {
        private IWriteJsonLogic<T> writeJsonLogic;
        private IReadJsonLogic<T> readJsonLogic;

        public IShiftTraceLogger TraceLogger { get; set; }

        
        protected BaseJsonConverter(IShiftTraceLogger logger, IWriteJsonLogic<T> writeJsonLogic, IReadJsonLogic<T> readJsonLogic)
        {
            this.writeJsonLogic = writeJsonLogic;
            this.readJsonLogic = readJsonLogic;
            TraceLogger = logger;
        }

        protected BaseJsonConverter(IShiftTraceLogger logger)
            : this (logger,
                  new WriteJsonLogic<T>() { TraceLogger = logger},
                  new ReadJsonLogic<T>() { TraceLogger = logger})
        {
        }

        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
        {
            writeJsonLogic.TraceLogger = TraceLogger;
            writeJsonLogic.WriteJson(writer, value, serializer);
        }

        public override T ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            readJsonLogic.TraceLogger = TraceLogger;
            return readJsonLogic.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        }


    }

}
