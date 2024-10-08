using Newtonsoft.Json.Serialization;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class TypeBinder : ISerializationBinder
    {
        private List<(Type type, string name)> typesNames;
        public static IShiftTraceLogger TraceLogger;
        public TypeBinder(List<(Type type, string name)> typesNames, IShiftTraceLogger traceLogger = null)
        {
            this.typesNames = typesNames;
            TraceLogger = traceLogger;
        }

        public void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
        {
            assemblyName = null;
            if (typesNames.Any(x => x.type == serializedType))
            {
                typeName = typesNames.Single(x => x.type == serializedType).name;
            }
            else
            {
                typeName = serializedType.FullName;
            }
        }

        public Type BindToType(string? assemblyName, string typeName)
        {
            return typesNames.SingleOrDefault(x => x.name == typeName).type;

        }

    }
}
