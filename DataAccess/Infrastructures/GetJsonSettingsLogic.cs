using DataAccess.DTOs;
using DataAccess.JsonConverters;
using Newtonsoft.Json;
using NLog;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public class GetJsonSettingsLogic : IGetJsonSettingsLogic
    {
        public IShiftTraceLogger? TraceLogger { get; set; }

        public GetJsonSettingsLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public JsonSerializerSettings GetSettings()
        {
            List<(Type type, string name)> typesNames = TypeBinderListFactory.GetTypeNameList(TypeFileVersion.version_v1);
            TypeBinder typeBinder = new(typesNames);
            List<JsonConverter> jsonConverters = GetConverters();

            var settings = new JsonSerializerSettings
            {
                Converters = jsonConverters,
                SerializationBinder = typeBinder,
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Include
            };
            return settings;
        }

        private List<JsonConverter> GetConverters()
        {
            return new List<JsonConverter>
            {
                    new FileVersionDTOJsonConverter(TraceLogger),
                    new ProjectDTOJsonConverter(TraceLogger)
                };
        }
    }
}
