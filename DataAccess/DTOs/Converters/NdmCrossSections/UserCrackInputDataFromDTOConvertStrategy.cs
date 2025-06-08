using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace DataAccess.DTOs
{
    public class UserCrackInputDataFromDTOConvertStrategy : ConvertStrategy<UserCrackInputData, UserCrackInputDataDTO>
    {
        private IUpdateStrategy<IUserCrackInputData> updateStrategy;

        public UserCrackInputDataFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            ReferenceDictionary = referenceDictionary;
            TraceLogger = traceLogger;
        }

        public override UserCrackInputData GetNewItem(UserCrackInputDataDTO source)
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

        private void GetNewItemBySource(IUserCrackInputData source)
        {
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new UserCrackInputDataUpdateStrategy();
        }
    }
}
