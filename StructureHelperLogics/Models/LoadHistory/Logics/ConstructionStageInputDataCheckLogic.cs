using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class ConstructionStageInputDataCheckLogic : CheckEntityLogic<IConstructionStageInputData>
    {
        private bool result;
        private HasPrimitivesCheckLogic checkPrimitiveCollectionLogic;

        public ConstructionStageInputDataCheckLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public override bool Check()
        {
            result = true;
            if (Entity is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": Construction stage input data";
                TraceMessage(errorString);
                throw new StructureHelperException(errorString);
            }
            if (Entity.Primitives is null || Entity.Primitives.Count == 0)
            {
                TraceMessage($"Construction stage {Entity.ConstructionStage.Name} does not contain any primitives");
                result = false;
            }
            CheckPrimitives();
            if (Entity.ForceAction is null)
            {
                TraceMessage($"Construction stage {Entity.ConstructionStage.Name} does not contain force action");
                result = false;
            }
            return result;
        }

        private void CheckPrimitives()
        {
            CheckRebarPrimitiveLogic checkRebarPrimitiveLogic = new()
            {
                CheckRebarHostMaterial = false,
                CheckRebarPlacement = false
            };
            checkPrimitiveCollectionLogic ??= new HasPrimitivesCheckLogic(
                TraceLogger,
                checkRebarPrimitiveLogic)
            {
                Entity = Entity,
            };
            if (checkPrimitiveCollectionLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(checkPrimitiveCollectionLogic.CheckResult);
        }
    }
}
