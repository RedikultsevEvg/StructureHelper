using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceCalculatorInputDataFromDTOConvertStrategy : ConvertStrategy<ForceCalculatorInputData, ForceCalculatorInputDataDTO>
    {
        private IUpdateStrategy<IForceCalculatorInputData> updateStrategy = new ForceCalculatorInputDataUpdateStrategy();
        private IHasPrimitivesProcessLogic primitivesProcessLogic = new HasPrimitivesProcessLogic();
        private IHasForceActionsProcessLogic actionsProcessLogic = new HasForceActionsProcessLogic();

        public override ForceCalculatorInputData GetNewItem(ForceCalculatorInputDataDTO source)
        {
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            ProcessPrimitives(source);
            ProcessActions(source);
            return NewItem;
        }

        private void ProcessPrimitives(IHasPrimitives source)
        {
            primitivesProcessLogic.Source = source;
            primitivesProcessLogic.Target = NewItem;
            primitivesProcessLogic.ReferenceDictionary = ReferenceDictionary;
            primitivesProcessLogic.TraceLogger = TraceLogger;
            primitivesProcessLogic.Process();
        }

        private void ProcessActions(IHasForceActions source)
        {
            actionsProcessLogic.Source = source;
            actionsProcessLogic.Target = NewItem;
            actionsProcessLogic.ReferenceDictionary = ReferenceDictionary;
            actionsProcessLogic.TraceLogger = TraceLogger;
            actionsProcessLogic.Process();
        }
    }
}
