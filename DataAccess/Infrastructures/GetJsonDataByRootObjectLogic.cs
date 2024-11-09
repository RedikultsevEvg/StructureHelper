using DataAccess.DTOs;
using Newtonsoft.Json;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public class GetJsonDataByRootObjectLogic : IGetJsonDataByRootObjectLogic
    {
        private IGetJsonSettingsLogic getJsonSettingsLogic;

        public GetJsonDataByRootObjectLogic(IGetJsonSettingsLogic getJsonSettingsLogic)
        {
            this.getJsonSettingsLogic = getJsonSettingsLogic;
        }

        public GetJsonDataByRootObjectLogic(IShiftTraceLogger traceLogger) : this (new GetJsonSettingsLogic(traceLogger))
        {
            
        }

        public IRootObjectDTO RootObject { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public string GetJsonData()
        {
            var rootString = Serialize(RootObject);
            return rootString;
        }

        private string Serialize(object obj)
        {
            JsonSerializerSettings settings = getJsonSettingsLogic.GetSettings();
            string serializedObject = JsonConvert.SerializeObject(obj, settings);
            return serializedObject;
        }
    }
}
