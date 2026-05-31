using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class HasPrimitivesCheckLogic : ICheckEntityLogic<IHasPrimitives>
    {
        private const string collectionDoesntHaveAnyPrimitives = "Calculator does not contain any primitives\n";
        private const string checkRebarLogic = ": check rebar logic";

        private string checkResult;
        private bool result;
        private ICheckEntityLogic<IRebarNdmPrimitive> checkRebarPrimitiveLogic;

        public IHasPrimitives Entity { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public HasPrimitivesCheckLogic(
            IShiftTraceLogger shiftTraceLogger,
            ICheckEntityLogic<IRebarNdmPrimitive> checkRebarPrimitiveLogic)
        {
            TraceLogger = shiftTraceLogger;
            this.checkRebarPrimitiveLogic = checkRebarPrimitiveLogic;
        }

        public HasPrimitivesCheckLogic() : this (new ShiftTraceLogger(), new CheckRebarPrimitiveLogic())
        {
            
        }

        public HasPrimitivesCheckLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public bool Check()
        {
            result = true;
            checkResult = string.Empty;
            CheckPrimitives();
            return result;
        }

        private void CheckPrimitives()
        {
            if (Entity.Primitives is null || (!Entity.Primitives.Any()))
            {
                result = false;
                checkResult += collectionDoesntHaveAnyPrimitives;
                TraceLogger?.AddMessage(collectionDoesntHaveAnyPrimitives, TraceLogStatuses.Error);
            }
            else
            {
                
                foreach (var primitive in Entity.Primitives)
                {
                    if (primitive.NdmElement.HeadMaterial is null)
                    {
                        result = false;
                        string message = $"Primitive {primitive.Name} does not have material\n";
                        checkResult += message;
                        TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                    }
                    if (primitive is IRebarNdmPrimitive rebar)
                    {
                        CheckRebar(rebar);
                    }
                }
            }
        }

        private void CheckRebar(IRebarNdmPrimitive rebar)
        {
            if (checkRebarPrimitiveLogic is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + checkRebarLogic);
            }
            checkRebarPrimitiveLogic.Entity = rebar;
            checkRebarPrimitiveLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger();
            if (checkRebarPrimitiveLogic.Check() == false)
            {
                result = false;
                checkResult += checkRebarPrimitiveLogic.CheckResult;
                return;
            }
            bool isPrimitivesContainRebarHost = Entity.Primitives.Contains(rebar.HostPrimitive);
            if (isPrimitivesContainRebarHost == false)
            {
                result = false;
                string message = $"Host {rebar.Name} ({rebar.HostPrimitive.Name}) is not included in primitives\n";
                checkResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
            }
        }
    }
}
