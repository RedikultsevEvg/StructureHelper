using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic of checking of input data for crack calcultor 
    /// </summary>
    public class CheckCrackCalculatorInputDataLogic : ICheckInputDataLogic<CrackInputData>
    {
        private bool result;

        public CrackInputData InputData {  get; set; }


        public string CheckResult { get; private set; }

        public IShiftTraceLogger? TraceLogger { get; set; }
        public CheckCrackCalculatorInputDataLogic(CrackInputData inputData)
        {
            InputData = inputData;
            CheckResult = string.Empty;
        }
        public bool Check()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Debug);
            result = true;
            CheckResult = string.Empty;
            CheckPrimitives();
            CheckActions();
            return result;
        }

        private void CheckActions()
        {
            if (InputData.ForceActions is null || (!InputData.ForceActions.Any()))
            {
                result = false;
                string message = "Calculator does not contain any actions\n";
                CheckResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
            };
        }

        private void CheckPrimitives()
        {
            if (InputData.Primitives is null || (!InputData.Primitives.Any()))
            {
                result = false;
                string message = "Calculator does not contain any primitives\n";
                CheckResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
            }
            else
            {
                foreach (var primitive in InputData.Primitives)
                {
                    if (primitive is RebarPrimitive rebar)
                    {
                        CheckRebar(rebar);
                    }
                }
            }
        }

        private void CheckRebar(RebarPrimitive rebar)
        {
            if (rebar.HostPrimitive is null)
            {
                result = false;
                string message = $"Primitive {rebar.Name} does not have a host\n";
                CheckResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
            }
            else
            {
                bool isPrimitivesContainRebarHost = InputData.Primitives.Contains(rebar.HostPrimitive);
                if (isPrimitivesContainRebarHost == false)
                {
                    result = false;
                    string message = $"Host {rebar.Name} ({rebar.HostPrimitive.Name}) is not included in primitives\n";
                    CheckResult += message;
                    TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                }
            }
            if (rebar.HostPrimitive.HeadMaterial.HelperMaterial is not ICrackedMaterial)
            {
                result = false;
                string message = $"Material of host of {rebar.Name} ({rebar.HostPrimitive.HeadMaterial.Name})  does not support cracking\n";
                CheckResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
            }
        }
    }
}
