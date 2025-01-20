using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;

namespace DataAccess.DTOs
{
    public class ForceActionToDTOConvertStrategy : ConvertStrategy<IForceAction, IForceAction>
    {
        private IConvertStrategy<ForceFactoredListDTO, IForceFactoredList> forceFactoredListConvertStrategy;
        private IConvertStrategy<ForceCombinationListDTO, IForceCombinationList> forceCombinationListConvertStrategy;
        private IConvertStrategy<ForceCombinationFromFileDTO, IForceCombinationFromFile> forceCombinationFromFileConvertStrategy;

        public ForceActionToDTOConvertStrategy(
            IConvertStrategy<ForceFactoredListDTO, IForceFactoredList> forceFactoredListConvertStrategy,
            IConvertStrategy<ForceCombinationListDTO, IForceCombinationList> forceCombinationListConvertStrategy,
            IConvertStrategy<ForceCombinationFromFileDTO, IForceCombinationFromFile> forceCombinationFromFileConvertStrategy)
        {
            this.forceFactoredListConvertStrategy = forceFactoredListConvertStrategy;
            this.forceCombinationListConvertStrategy = forceCombinationListConvertStrategy;
            this.forceCombinationFromFileConvertStrategy = forceCombinationFromFileConvertStrategy;
        }

        public ForceActionToDTOConvertStrategy() {  }

        public override IForceAction GetNewItem(IForceAction source)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage($"Force action converting has been started");
            InitializeStrategies();
            if (source is IForceFactoredList forceFactoredList)
            {
                return GetForceCombinationByFactor(forceFactoredList);
            }
            else if (source is IForceCombinationList forceCombinationList)
            {
                return GetForceCombinationList(forceCombinationList);
            }
            else if (source is IForceCombinationFromFile forceCombinationFile)
            {
                return GetForceCombinationFile(forceCombinationFile);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                TraceLogger.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }

        private IForceAction GetForceCombinationFile(IForceCombinationFromFile forceCombinationFile)
        {
            var convertLogic = new DictionaryConvertStrategy<ForceCombinationFromFileDTO, IForceCombinationFromFile>(this, forceCombinationFromFileConvertStrategy);
            var forceCombination = convertLogic.Convert(forceCombinationFile);
            return forceCombination;
        }

        private void InitializeStrategies()
        {
            forceFactoredListConvertStrategy ??= new ForceFactoredListToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
            forceCombinationListConvertStrategy ??= new ForceCombinationListToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger }; ;
            forceCombinationFromFileConvertStrategy ??= new ForceCombinaionFromFileToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger }; ;
        }

        private ForceCombinationListDTO GetForceCombinationList(IForceCombinationList forceCombinationList)
        {
            var convertLogic = new DictionaryConvertStrategy<ForceCombinationListDTO, IForceCombinationList>(this, forceCombinationListConvertStrategy);
            var forceCombination = convertLogic.Convert(forceCombinationList);
            return forceCombination;
        }

        private ForceFactoredListDTO GetForceCombinationByFactor(IForceFactoredList forceCombinationByFactor)
        {
            var convertLogic = new DictionaryConvertStrategy<ForceFactoredListDTO, IForceFactoredList>(this, forceFactoredListConvertStrategy);
            var forceCombination = convertLogic.Convert(forceCombinationByFactor);
            return forceCombination;
        }
    }
}
