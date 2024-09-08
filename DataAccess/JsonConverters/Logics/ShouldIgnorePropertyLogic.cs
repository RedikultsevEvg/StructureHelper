using Newtonsoft.Json;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.JsonConverters
{
    public class ShouldIgnorePropertyLogic : IShouldIgnorePropertyLogic
    {
        public IShiftTraceLogger? TraceLogger { get; set; }
        public bool ShouldIgnoreProperty(PropertyInfo prop)
        {
            // Check for [JsonIgnore] attribute
            var jsonIgnoreAttribute = prop.GetCustomAttribute<JsonIgnoreAttribute>();
            return jsonIgnoreAttribute != null;
        }
    }
}
