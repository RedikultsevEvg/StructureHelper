using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives.Logics
{
    public class CheckRebarPrimitiveLogic : ICheckRebarPrimitiveLogic
    {
        private string checkResult;
        private bool result;

        public IRebarNdmPrimitive RebarPrimitive { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckRebarPrimitiveLogic(IShiftTraceLogger traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public CheckRebarPrimitiveLogic() : this (new ShiftTraceLogger())
        {
            
        }

        public bool Check()
        {
            result = true;
            checkResult = string.Empty;
            CheckRebar();
            return result;
        }

        private void CheckRebar()
        {
            if (RebarPrimitive.HostPrimitive is null)
            {
                result = false;
                string message = $"Primitive {RebarPrimitive.Name} does not have a host\n";
                checkResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                return;
            }

            if (RebarPrimitive.HostPrimitive is IHasDivisionSize division)
            {
                if (!division.IsPointInside(RebarPrimitive.Center))
                {
                    result = false;
                    string message = $"Primitive of rebar {RebarPrimitive.Name} is out of its host {RebarPrimitive.HostPrimitive.Name}";
                    checkResult += message;
                    TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                }
            }

            if (RebarPrimitive.HostPrimitive.NdmElement.HeadMaterial.HelperMaterial is not ICrackedMaterial)
            {
                result = false;
                string message = $"Material of host of {RebarPrimitive.Name} ({RebarPrimitive.HostPrimitive.NdmElement.HeadMaterial.Name})  does not support cracking\n";
                checkResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
            }
        }
    }
}
