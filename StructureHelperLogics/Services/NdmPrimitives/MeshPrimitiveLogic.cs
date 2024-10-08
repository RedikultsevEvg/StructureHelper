using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public class MeshPrimitiveLogic : IMeshPrimitiveLogic
    {
        public INdmPrimitive Primitive { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public ITriangulationOptions TriangulationOptions { get; set; }

        public List<INdm> MeshPrimitive()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            List<INdm> ndmCollection = new();
            var itemNdms = Primitive.GetNdms(TriangulationOptions);
            ndmCollection.AddRange(itemNdms);
            TraceLogger?.AddMessage($"Triangulation of primitive {Primitive.Name} has finished, {itemNdms.Count()} part(s) were obtained", TraceLogStatuses.Service);
            return ndmCollection;
        }
    }
}
