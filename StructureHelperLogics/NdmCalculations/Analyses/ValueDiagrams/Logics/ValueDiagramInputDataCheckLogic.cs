using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramInputDataCheckLogic : CheckEntityLogic<IValueDiagramCalculatorInputData>
    {
        private ICheckEntityLogic<IValueDiagramEntity> checkEntityLogic;
        private ICheckEntityLogic<IHasPrimitives> primitivesCheckLogic;
        private ICheckEntityLogic<IEnumerable<IForceAction>> actionsCheckLogic;

        private bool result;

        public ValueDiagramInputDataCheckLogic(
            ICheckEntityLogic<IValueDiagramEntity> checkEntityLogic,
            ICheckEntityLogic<IHasPrimitives> primitivesCheckLogic,
            ICheckEntityLogic<IEnumerable<IForceAction>> actionsCheckLogic)
        {
            this.checkEntityLogic = checkEntityLogic;
            this.primitivesCheckLogic = primitivesCheckLogic;
            this.actionsCheckLogic = actionsCheckLogic;
        }

        public ValueDiagramInputDataCheckLogic()
        {
            checkEntityLogic = new ValueDiagramEntityCheckLogic(TraceLogger);
            CheckRebarPrimitiveLogic checkRebarPrimitiveLogic = new()
            {
                CheckRebarHostMaterial = false,
                CheckRebarPlacement = false
            };
            primitivesCheckLogic = new HasPrimitivesCheckLogic(TraceLogger, checkRebarPrimitiveLogic);
            actionsCheckLogic = new CheckForceActionsLogic(TraceLogger);
        }

        public override bool Check()
        {
            result = true;
            if (Entity is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": input data";
                TraceMessage(errorString);
                throw new StructureHelperException(errorString);
            }
            if (Entity.Primitives is null || !Entity.Primitives.Any())
            {
                TraceMessage("Calculator does not contain any primitives");
                result = false;
            }
            if (Entity.ForceActions is null || !Entity.ForceActions.Any())
            {
                TraceMessage("Calculator does not contain any forces");
                result = false;
            }
            if (Entity.Diagrams is null || !Entity.Diagrams.Any() || !Entity.Diagrams.Where(x=> x.IsTaken == true).Any())
            {
                TraceMessage("Calculator does not contain any diagrams for calculation");
                result = false;
            }
            foreach (var entity in Entity.Diagrams)
            {
                checkEntityLogic.Entity = entity;
                bool isValid = checkEntityLogic.Check();
                if (isValid == false)
                {
                    result = false;
                    TraceMessage(checkEntityLogic.CheckResult);
                }
            }
            CheckPrimitives();
            CheckActions();
            return result;
        }

        private void CheckPrimitives()
        {
            primitivesCheckLogic.Entity = Entity;
            if (primitivesCheckLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(primitivesCheckLogic.CheckResult);

        }

        private void CheckActions()
        {
            actionsCheckLogic.Entity = Entity.ForceActions;
            if (actionsCheckLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(actionsCheckLogic.CheckResult);
        }
    }
}
