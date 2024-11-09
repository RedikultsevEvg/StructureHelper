using DataAccess.DTOs;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public class GetRootObjectLogic : IGetRootObjectByJsonDataLogic
    {
        private IGetJsonSettingsLogic jsonSettingsLogic;

        public GetRootObjectLogic(
            IGetJsonSettingsLogic jsonSettingsLogic,
            IShiftTraceLogger traceLogger)
        {
            this.jsonSettingsLogic = jsonSettingsLogic;
            TraceLogger = traceLogger;
        }

        public GetRootObjectLogic(IShiftTraceLogger traceLogger) : this (new GetJsonSettingsLogic(traceLogger), traceLogger)
        {
            
        }

        public string JsonData { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public RootObjectDTO? GetRootObject()
        {
            jsonSettingsLogic.TraceLogger = TraceLogger;
            JsonSerializerSettings settings = jsonSettingsLogic.GetSettings();
            TraceLogger?.AddMessage("Deserialization of data is started");
            RootObjectDTO rootObject;
            try
            {
                rootObject = JsonConvert.DeserializeObject<RootObjectDTO>(JsonData, settings);
            }
            catch (Exception ex)
            {
                string errorString = "Error of deserialization of json data" + ex.Message;
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            TraceLogger?.AddMessage("Deserialization of data has been finished successfully");
            return rootObject;
        }
    }
}
