using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceActionsFromDTOConvertStrategy : ConvertStrategy<IForceAction, IForceAction>
    {
        private IConvertStrategy<ForceCombinationList, ForceCombinationListDTO> listConvertStrategy;
        private IConvertStrategy<ForceFactoredList, ForceCombinationByFactorV1_0DTO> factorConvertStrategy_v1_0;
        private IConvertStrategy<ForceFactoredList, ForceCombinationByFactorDTO> factorConvertStrategy;

        public ForceActionsFromDTOConvertStrategy(
            IConvertStrategy<ForceCombinationList, ForceCombinationListDTO> listConvertStrategy,
            IConvertStrategy<ForceFactoredList, ForceCombinationByFactorV1_0DTO> factorConvertStrategy_v1_0,
            IConvertStrategy<ForceFactoredList, ForceCombinationByFactorDTO> factorConvertStrategy)
        {
            this.listConvertStrategy = listConvertStrategy;
            this.factorConvertStrategy_v1_0 = factorConvertStrategy_v1_0;
            this.factorConvertStrategy = factorConvertStrategy;
        }

        public ForceActionsFromDTOConvertStrategy() : this (
            new ForceCombinationListFromDTOConvertStrategy(),
            new ForceCombinationByFactorV1_0FromDTOConvertStrategy(),
            new ForceCombinationByFactorFromDTOConvertStrategy())
        {
            
        }

        public override IForceAction GetNewItem(IForceAction source)
        {
            if (source is ForceCombinationByFactorV1_0DTO combination_v1_0)
            {
                return Obsolete_GetForceCombination_V1_0(combination_v1_0);
            }
            if (source is ForceCombinationByFactorDTO combination)
            {
                return GetForceCombination(combination);
            }
            if (source is ForceCombinationListDTO forceList)
            {
                return GetForceList(forceList);
            }
            string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
            TraceLogger.AddMessage(errorString, TraceLogStatuses.Error);
            throw new StructureHelperException(errorString);
        }

        private ForceFactoredList Obsolete_GetForceCombination_V1_0(ForceCombinationByFactorV1_0DTO source)
        {
            TraceLogger?.AddMessage("Force action is combination by factors version 1.0 (obsolete)", TraceLogStatuses.Warning);
            factorConvertStrategy_v1_0.ReferenceDictionary = ReferenceDictionary;
            factorConvertStrategy_v1_0.TraceLogger = TraceLogger;
            ForceFactoredList newItem = factorConvertStrategy_v1_0.Convert(source);
            return newItem;
        }

        private IForceAction GetForceCombination(ForceCombinationByFactorDTO source)
        {
            TraceLogger?.AddMessage("Force action is combination by factors");
            factorConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            factorConvertStrategy.TraceLogger = TraceLogger;
            ForceFactoredList newItem = factorConvertStrategy.Convert(source);
            return newItem;
        }


        private IForceAction GetForceList(ForceCombinationListDTO forceList)
        {
            TraceLogger?.AddMessage("Force action is combination by list");
            listConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            listConvertStrategy.TraceLogger = TraceLogger;
            ForceCombinationList newItem = listConvertStrategy.Convert(forceList);
            return newItem;
        }
    }
}
