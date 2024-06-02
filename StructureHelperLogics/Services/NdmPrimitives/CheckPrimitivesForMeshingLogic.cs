using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public class CheckPrimitivesForMeshingLogic : ICheckPrimitivesForMeshingLogic
    {
        public IEnumerable<INdmPrimitive> Primitives { get; set; }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool Check()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            if (!Primitives.Any())
            {
                string errorMessage = string.Intern(ErrorStrings.DataIsInCorrect + $": Count of primitive must be greater than zero");
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
            if (!Primitives.Any(x => x.Triangulate == true))
            {
                string errorMessage = string.Intern(ErrorStrings.DataIsInCorrect + $": There are not primitives to triangulate");
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
            foreach (var item in Primitives)
            {
                if (item.Triangulate == true &
                    item.HeadMaterial is null)
                {
                    string errorMessage = string.Intern(ErrorStrings.DataIsInCorrect + $": Primitive: {item.Name} can't be triangulated since material is null");
                    TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                    throw new StructureHelperException(errorMessage);
                }
            }
            return true;
        }
    }
}
