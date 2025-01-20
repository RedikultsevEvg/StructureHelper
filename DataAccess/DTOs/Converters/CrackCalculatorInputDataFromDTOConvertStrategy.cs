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
        private IUpdateStrategy<IUserCrackInputData> userDataUpdateStrategy;
        private IHasPrimitivesProcessLogic primitivesProcessLogic;
        private IHasForceActionsProcessLogic actionsProcessLogic;
        private IConvertStrategy<UserCrackInputData, UserCrackInputDataDTO> userDataConvertStrategy;

        public override CrackCalculatorInputData GetNewItem(CrackCalculatorInputDataDTO source)
        {
            InitializeStrategies();
            try
            {
                GetNewItemBySource(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }
        private void GetNewItemBySource(CrackCalculatorInputDataDTO source)
        {
            NewItem = new(source.Id);
            ProcessPrimitives(source);
            ProcessActions(source);
            NewItem.UserCrackInputData = new UserCrackInputData(source.UserCrackInputData.Id);
            userDataUpdateStrategy.Update(NewItem.UserCrackInputData, source.UserCrackInputData);
        }
        private void InitializeStrategies()
        {
            userDataUpdateStrategy ??= new UserCrackInputDataUpdateStrategy();
            primitivesProcessLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
            actionsProcessLogic ??= new HasForceActionsProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            userDataConvertStrategy ??= new UserCrackInputDataFromDTOConvertStrategy(ReferenceDictionary, TraceLogger);
        }
        private void ProcessPrimitives(IHasPrimitives source)
        {
            primitivesProcessLogic.Source = source;
            primitivesProcessLogic.Target = NewItem;
            primitivesProcessLogic.Process();
        }
        private void ProcessActions(IHasForceActions source)
        {
            actionsProcessLogic.Source = source;
            actionsProcessLogic.Target = NewItem;
            actionsProcessLogic.Process();
        }
    }
}
