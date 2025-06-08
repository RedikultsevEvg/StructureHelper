using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;

namespace DataAccess.DTOs
{
    public class ForceActionFromDTOConvertStrategy : ConvertStrategy<IForceAction, IForceAction>
    {
        private IConvertStrategy<ForceCombinationList, ForceCombinationListDTO> listConvertStrategy;
        private IConvertStrategy<ForceFactoredList, ForceCombinationByFactorV1_0DTO> factorConvertStrategy_v1_0;
        private IConvertStrategy<ForceFactoredList, ForceFactoredListDTO> factorConvertStrategy;
        private IConvertStrategy<ForceCombinationFromFile, ForceCombinationFromFileDTO> fileConvertStrategy;

        public ForceActionFromDTOConvertStrategy(
            IConvertStrategy<ForceCombinationList, ForceCombinationListDTO> listConvertStrategy,
            IConvertStrategy<ForceFactoredList, ForceCombinationByFactorV1_0DTO> factorConvertStrategy_v1_0,
            IConvertStrategy<ForceFactoredList, ForceFactoredListDTO> factorConvertStrategy,
            IConvertStrategy<ForceCombinationFromFile, ForceCombinationFromFileDTO> fileConvertStrategy)
        {
            this.listConvertStrategy = listConvertStrategy;
            this.factorConvertStrategy_v1_0 = factorConvertStrategy_v1_0;
            this.factorConvertStrategy = factorConvertStrategy;
            this.fileConvertStrategy = fileConvertStrategy;
        }

        public ForceActionFromDTOConvertStrategy() { }

        public override IForceAction GetNewItem(IForceAction source)
        {
            InitializeStrategies();
            try
            {
                NewItem = GetNewItemBySource(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void InitializeStrategies()
        {
            listConvertStrategy ??= new ForceCombinationListFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            factorConvertStrategy ??= new ForceFactoredListFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            fileConvertStrategy ??= new ForceCombinationFromFileFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
        }
        private IForceAction GetNewItemBySource(IForceAction source)
        {
            if (source is ForceFactoredListDTO combination)
            {
                return GetFactoredCombination(combination);
            }
            if (source is ForceCombinationListDTO forceList)
            {
                return GetForceList(forceList);
            }
            if (source is ForceCombinationFromFileDTO fileCombination)
            {
                return GetFileCombination(fileCombination);
            }
            if (source is ForceCombinationByFactorV1_0DTO combination_v1_0)
            {
                return Obsolete_GetForceCombination_V1_0(combination_v1_0);
            }
            string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
            TraceLogger.AddMessage(errorString, TraceLogStatuses.Error);
            throw new StructureHelperException(errorString);
        }
        private ForceCombinationFromFile GetFileCombination(ForceCombinationFromFileDTO source)
        {
            TraceLogger?.AddMessage("Force action is combination by factors");
            ForceCombinationFromFile newItem = fileConvertStrategy.Convert(source);
            return newItem;
        }
        private ForceFactoredList Obsolete_GetForceCombination_V1_0(ForceCombinationByFactorV1_0DTO source)
        {
            TraceLogger?.AddMessage("Force action is combination by factors version 1.0 (obsolete)", TraceLogStatuses.Warning);
            factorConvertStrategy_v1_0 ??= new ForceCombinationByFactorV1_0FromDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            ForceFactoredList newItem = factorConvertStrategy_v1_0.Convert(source);
            return newItem;
        }
        private IForceAction GetFactoredCombination(ForceFactoredListDTO source)
        {
            TraceLogger?.AddMessage("Force action is combination by factors");
            ForceFactoredList newItem = factorConvertStrategy.Convert(source);
            return newItem;
        }
        private IForceAction GetForceList(ForceCombinationListDTO forceList)
        {
            TraceLogger?.AddMessage("Force action is combination by list");
            ForceCombinationList newItem = listConvertStrategy.Convert(forceList);
            return newItem;
        }
    }
}
