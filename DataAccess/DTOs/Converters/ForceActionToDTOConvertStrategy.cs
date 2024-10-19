using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;

namespace DataAccess.DTOs.Converters
{
    public class ForceActionToDTOConvertStrategy : IConvertStrategy<IForceAction, IForceAction>
    {
        private IConvertStrategy<ForceCombinationByFactorDTO, IForceCombinationByFactor> forceCombinationByFactorConvertStrategy;
        private IConvertStrategy<ForceCombinationListDTO, IForceCombinationList> forceCombinationListConvertStrategy;

        public ForceActionToDTOConvertStrategy(
            IConvertStrategy<ForceCombinationByFactorDTO, IForceCombinationByFactor> forceCombinationByFactorConvertStrategy,
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

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public IForceAction Convert(IForceAction source)
        {
            try
            {
                return ProcessForceAction(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private IForceAction ProcessForceAction(IForceAction source)
        {
            if (source is IForceCombinationByFactor forceCombinationByFactor)
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

        private ForceCombinationByFactorDTO GetForceCombinationByFactor(IForceCombinationByFactor forceCombinationByFactor)
        {
            forceCombinationByFactorConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            forceCombinationByFactorConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ForceCombinationByFactorDTO, IForceCombinationByFactor>(this, forceCombinationByFactorConvertStrategy);
            var forceCombination = convertLogic.Convert(forceCombinationByFactor);
            return forceCombination;
        }
    }
}
