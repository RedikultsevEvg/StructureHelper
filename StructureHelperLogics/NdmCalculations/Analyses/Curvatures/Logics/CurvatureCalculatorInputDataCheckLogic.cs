using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorInputDataCheckLogic : CheckEntityLogic<ICurvatureCalculatorInputData>
    {
        private ICheckEntityLogic<IHasPrimitives> primitivesCheckLogic;
        private ICheckEntityLogic<IEnumerable<IForceAction>> actionsCheckLogic;
        private bool result;

        private ICheckEntityLogic<IEnumerable<IForceAction>> ActionsCheckLogic => actionsCheckLogic ??= new CheckForceActionsLogic(TraceLogger);

        private ICheckEntityLogic<IHasPrimitives> PrimitivesCheckLogic => primitivesCheckLogic ??= new HasPrimitivesCheckLogic();

        public CurvatureCalculatorInputDataCheckLogic(IShiftTraceLogger traceLogger)
        {
            TraceLogger = traceLogger;
            CheckRebarPrimitiveLogic checkRebarPrimitiveLogic = new()
            {
                CheckRebarHostMaterial = true,
                CheckRebarPlacement = true
            };
            primitivesCheckLogic = new HasPrimitivesCheckLogic(TraceLogger, checkRebarPrimitiveLogic);
        }

        public override bool Check()
        {
            result = true;
            if (Entity is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": curvature calculator input data";
                TraceMessage(errorString);
                throw new StructureHelperException(errorString);
            }
            if (Entity.Primitives is null || Entity.Primitives.Count == 0)
            {
                TraceMessage("Calculator does not contain any primitives");
                result = false;
            }
            if (Entity.ForceActions is null || Entity.ForceActions.Count == 0)
            {
                TraceMessage("Calculator does not contain any forces");
                result = false;
            }
            CheckPrimitives();
            CheckActions();
            return result;
        }

        private void CheckPrimitives()
        {
            PrimitivesCheckLogic.Entity = Entity;
            if (PrimitivesCheckLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(PrimitivesCheckLogic.CheckResult);

        }

        private void CheckActions()
        {
            ActionsCheckLogic.Entity = Entity.ForceActions;
            if (ActionsCheckLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(ActionsCheckLogic.CheckResult);
        }
    }
}
