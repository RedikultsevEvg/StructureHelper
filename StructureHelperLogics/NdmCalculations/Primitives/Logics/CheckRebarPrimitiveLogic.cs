using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Materials;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <summary>
    /// Logic for checking of propertis of rebar
    /// </summary>
    public class CheckRebarPrimitiveLogic : ICheckEntityLogic<IRebarNdmPrimitive>
    {
        private string checkResult;
        private bool result;
        /// <summary>
        /// True if checking of rebar placement inside its host is required
        /// </summary>
        public bool CheckRebarPlacement { get; set; } = true;
        /// <summary>
        /// True if rebar must has its host
        /// </summary>
        public bool CheckRebarHostMaterial { get; set; } = true;
        /// <summary>
        /// Rebar primitive
        /// </summary>
        public IRebarNdmPrimitive Entity { get; set; }

        public string CheckResult => checkResult;
        /// <inheritdoc/>
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
            if (Entity.HostPrimitive is null)
            {
                result = false;
                string message = $"Primitive {Entity.Name} does not have a host\n";
                checkResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                return;
            }
            if (CheckRebarPlacement == true)
            {
                CheckIfRebarInsideHostPrimitive();
            }
            if (CheckRebarHostMaterial == true)
            {
                CheckIfRemarMaterialIsCrackedMaterial();
            }
        }

        private void CheckIfRemarMaterialIsCrackedMaterial()
        {
            if (Entity.HostPrimitive.NdmElement.HeadMaterial.HelperMaterial is not ICrackedMaterial)
            {
                result = false;
                string message = $"Material of host of {Entity.Name} ({Entity.HostPrimitive.NdmElement.HeadMaterial.Name})  does not support cracking\n";
                checkResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
            }
        }

        private void CheckIfRebarInsideHostPrimitive()
        {
            if (Entity.HostPrimitive is IHasDivisionSize division)
            {
                if (!division.IsPointInside(Entity.Center))
                {
                    result = false;
                    string message = $"Primitive of rebar {Entity.Name} is out of its host {Entity.HostPrimitive.Name}";
                    checkResult += message;
                    TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                }
            }
        }
    }
}
