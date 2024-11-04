using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CrackCalculatorInputDataFromDTOConvertStrategy : ConvertStrategy<CrackCalculatorInputData, CrackCalculatorInputDataDTO>
    {
        private IUpdateStrategy<IUserCrackInputData> userDataUpdateStrategy = new UserCrackInputDataUpdateStrategy();
        private IHasPrimitivesProcessLogic primitivesProcessLogic = new HasPrimitivesProcessLogic();
        private IHasForceActionsProcessLogic actionsProcessLogic = new HasForceActionsProcessLogic();

        public override CrackCalculatorInputData GetNewItem(CrackCalculatorInputDataDTO source)
        {
            NewItem = new(source.Id);
            ProcessPrimitives(source);
            ProcessActions(source);
            NewItem.UserCrackInputData = new UserCrackInputData(source.UserCrackInputData.Id);
            userDataUpdateStrategy.Update(NewItem.UserCrackInputData, source.UserCrackInputData);
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
