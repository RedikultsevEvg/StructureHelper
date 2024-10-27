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
        private IConvertStrategy<ForceCombinationByFactor, ForceCombinationByFactorDTO> factorConvertStrategy;

        public ForceActionsFromDTOConvertStrategy(
            IConvertStrategy<ForceCombinationList, ForceCombinationListDTO> listConvertStrategy,
            IConvertStrategy<ForceCombinationByFactor, ForceCombinationByFactorDTO> factorConvertStrategy)
        {
            this.listConvertStrategy = listConvertStrategy;
            this.factorConvertStrategy = factorConvertStrategy;
        }

        public ForceActionsFromDTOConvertStrategy() : this (
            new ForceCombinationListFromDTOConvertStrategy(),
            new ForceCombinationByFactorFromDTOConvertStrategy())
        {
            
        }

        public override IForceAction GetNewItem(IForceAction source)
        {
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

        private IForceAction GetForceCombination(ForceCombinationByFactorDTO source)
        {
            TraceLogger?.AddMessage("Force action is combination by factors");
            factorConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            factorConvertStrategy.TraceLogger = TraceLogger;
            ForceCombinationByFactor newItem = factorConvertStrategy.Convert(source);
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
