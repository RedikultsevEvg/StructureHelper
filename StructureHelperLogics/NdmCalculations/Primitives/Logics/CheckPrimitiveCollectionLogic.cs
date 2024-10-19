using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
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
    public class CheckPrimitiveCollectionLogic : ICheckPrimitiveCollectionLogic
    {
        private const string collectionDoesntHaveAnyPrimitives = "Calculator does not contain any primitives\n";
        private const string checkRebarLogic = ": check rebar logic";

        private string checkResult;
        private bool result;
        private ICheckRebarPrimitiveLogic checkRebarPrimitiveLogic;

        public IHasPrimitives HasPrimitives { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckPrimitiveCollectionLogic(IShiftTraceLogger shiftTraceLogger, ICheckRebarPrimitiveLogic checkRebarPrimitiveLogic)
        {
            TraceLogger = shiftTraceLogger;
            this.checkRebarPrimitiveLogic = checkRebarPrimitiveLogic;
        }

        public CheckPrimitiveCollectionLogic() : this (new ShiftTraceLogger(), new CheckRebarPrimitiveLogic())
        {
            
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
            if (HasPrimitives.Primitives is null || (!HasPrimitives.Primitives.Any()))
            {
                result = false;
                checkResult += collectionDoesntHaveAnyPrimitives;
                TraceLogger?.AddMessage(collectionDoesntHaveAnyPrimitives, TraceLogStatuses.Error);
            }
            else
            {
                foreach (var primitive in HasPrimitives.Primitives)
                {
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
            checkRebarPrimitiveLogic.RebarPrimitive = rebar;
            checkRebarPrimitiveLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger();
            if (checkRebarPrimitiveLogic.Check() == false)
            {
                result = false;
                checkResult += checkRebarPrimitiveLogic.CheckResult;
                return;
            }
            bool isPrimitivesContainRebarHost = HasPrimitives.Primitives.Contains(rebar.HostPrimitive);
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
