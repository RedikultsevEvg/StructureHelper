using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;

namespace DataAccess.DTOs
{
    public class VersionItemFromDTOConvertStrategy : IConvertStrategy<ISaveable, ISaveable>
    {
        private const string AnalysisIs = "Analysis type is";
        private IConvertStrategy<ICrossSection, ICrossSection> crossSectionConvertStrategy;

        public VersionItemFromDTOConvertStrategy(IConvertStrategy<ICrossSection, ICrossSection> crossSectionConvertStrategy)
        {
            this.crossSectionConvertStrategy = crossSectionConvertStrategy;
        }

        public VersionItemFromDTOConvertStrategy() : this (new CrossSectionFromDTOConvertStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ISaveable Convert(ISaveable source)
        {
            try
            {
                Check();
                return GetNewAnalysis(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private ISaveable GetNewAnalysis(ISaveable source)
        {
            ISaveable newItem;
            if (source is ICrossSection crossSection)
            {
                newItem = ProcessCrossSection(crossSection);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            TraceLogger?.AddMessage($"Object of type <<{newItem.GetType()}>> was obtained", TraceLogStatuses.Service);
            return newItem;
        }

        private ICrossSection ProcessCrossSection(ICrossSection source)
        {
            TraceLogger?.AddMessage(AnalysisIs + " Cross-Section", TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Cross-Section converting is started", TraceLogStatuses.Service);
            crossSectionConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            crossSectionConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ICrossSection, ICrossSection>(this, crossSectionConvertStrategy);
            ICrossSection newItem = convertLogic.Convert(source);
            TraceLogger?.AddMessage("Cross-Section converting has been finished succesfully", TraceLogStatuses.Service);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<ISaveable, ISaveable>(this);
            checkLogic.Check();
        }
    }
}
