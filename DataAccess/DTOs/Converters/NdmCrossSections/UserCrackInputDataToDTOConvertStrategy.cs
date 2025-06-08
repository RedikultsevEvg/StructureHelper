using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class UserCrackInputDataToDTOConvertStrategy : ConvertStrategy<UserCrackInputDataDTO, IUserCrackInputData>
    {
        private IUpdateStrategy<IUserCrackInputData> updateStrategy;

        public UserCrackInputDataToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            ReferenceDictionary = referenceDictionary;
            TraceLogger = traceLogger;
        }

        public override UserCrackInputDataDTO GetNewItem(IUserCrackInputData source)
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
