using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;

namespace DataAccess.DTOs.Converters
{
    public class ForceActionToDTOConvertStrategy : ConvertStrategy<IForceAction, IForceAction>
    {
        private IConvertStrategy<ForceCombinationByFactorDTO, IForceFactoredList> forceCombinationByFactorConvertStrategy;
        private IConvertStrategy<ForceCombinationListDTO, IForceCombinationList> forceCombinationListConvertStrategy;

        public ForceActionToDTOConvertStrategy(
            IConvertStrategy<ForceCombinationByFactorDTO, IForceFactoredList> forceCombinationByFactorConvertStrategy,
            IConvertStrategy<ForceCombinationListDTO, IForceCombinationList> forceCombinationListConvertStrategy)
        {
            this.forceCombinationByFactorConvertStrategy = forceCombinationByFactorConvertStrategy;
            this.forceCombinationListConvertStrategy = forceCombinationListConvertStrategy;
        }

        public ForceActionToDTOConvertStrategy() : this(
            new ForceCombinationByFactorToDTOConvertStrategy(),
            new ForceCombinationListToDTOConvertStrategy())
        {
            
        }

        public override IForceAction GetNewItem(IForceAction source)
        {
            if (source is IForceFactoredList forceCombinationByFactor)
            {
                return GetForceCombinationByFactor(forceCombinationByFactor);
            }
            else if (source is IForceCombinationList forceCombinationList)
            {
                return GetForceCombinationList(forceCombinationList);
            }
            else
            {

                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                TraceLogger.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }

        private ForceCombinationListDTO GetForceCombinationList(IForceCombinationList forceCombinationList)
        {
            forceCombinationListConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            forceCombinationListConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ForceCombinationListDTO, IForceCombinationList>(this, forceCombinationListConvertStrategy);
            var forceCombination = convertLogic.Convert(forceCombinationList);
            return forceCombination;
        }

        private ForceCombinationByFactorDTO GetForceCombinationByFactor(IForceFactoredList forceCombinationByFactor)
        {
            forceCombinationByFactorConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            forceCombinationByFactorConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ForceCombinationByFactorDTO, IForceFactoredList>(this, forceCombinationByFactorConvertStrategy);
            var forceCombination = convertLogic.Convert(forceCombinationByFactor);
            return forceCombination;
        }
    }
}
