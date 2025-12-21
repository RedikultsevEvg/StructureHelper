using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Cracking.CheckLogics;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic of checking of input data for crack calcultor 
    /// </summary>
    public class CheckCrackCalculatorInputDataLogic : CheckEntityLogic<ICrackCalculatorInputData>
    {
        private bool result;
        private ICheckEntityLogic<IHasPrimitives> checkPrimitiveCollectionLogic;
        private ICheckEntityLogic<IEnumerable<INdmPrimitive>> checkMaterialsForCrackingLogic;
        private ICheckEntityLogic<IEnumerable<INdmPrimitive>> CheckMaterialsForCrackingLogic => checkMaterialsForCrackingLogic ??= new PrimitivesForCrackMaterialCheckLogic();
        private ICheckEntityLogic<IHasPrimitives> CheckPrimitiveCollectionLogic => checkPrimitiveCollectionLogic ??= new HasPrimitivesCheckLogic() ;

        public CheckCrackCalculatorInputDataLogic(ICheckEntityLogic<IHasPrimitives> checkPrimitiveCollectionLogic)
        {
            this.checkPrimitiveCollectionLogic = checkPrimitiveCollectionLogic;
        }

        public CheckCrackCalculatorInputDataLogic()
        {
            
        }

        public override bool Check()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            result = true;
            CheckResult = string.Empty;
            CheckCrackingMaterials();
            CheckPrimitives();
            CheckActions();
            return result;
        }

        private void CheckCrackingMaterials()
        {
            CheckMaterialsForCrackingLogic.TraceLogger = TraceLogger;
            CheckMaterialsForCrackingLogic.Entity = Entity.Primitives;
            if (CheckMaterialsForCrackingLogic.Check() == false)
            {
                result = false;
                CheckResult += CheckMaterialsForCrackingLogic.CheckResult;
            }
        }

        private void CheckPrimitives()
        {
            if (CheckPrimitiveCollectionLogic is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": check primitive logic");
            }
            CheckPrimitiveCollectionLogic.Entity = Entity;
            CheckPrimitiveCollectionLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger();
            if (CheckPrimitiveCollectionLogic.Check() == false)
            {
                result = false;
                CheckResult += CheckPrimitiveCollectionLogic.CheckResult;
                TraceLogger?.AddMessage(CheckPrimitiveCollectionLogic.CheckResult, TraceLogStatuses.Error);
            }
        }

        private void CheckActions()
        {
            if (Entity.ForceActions is null || (!Entity.ForceActions.Any()))
            {
                result = false;
                string message = "Calculator does not contain any actions\n";
                CheckResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                return;
            };
            var checkLogic = new CheckForceActionsLogic(TraceLogger)
            {
                Entity = Entity.ForceActions
            };
            if (checkLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(checkLogic.CheckResult);
        }
        private void TraceMessage(string errorString)
        {
            CheckResult += errorString + "\n";
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
