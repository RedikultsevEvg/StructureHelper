using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;

namespace DataAccess.DTOs
{
    public class VersionProcessorToDTOConvertStrategy : ConvertStrategy<VersionProcessorDTO, IVersionProcessor>
    {
        private IConvertStrategy<DateVersionDTO, IDateVersion> dataVersionConvertStrategy;

        public VersionProcessorToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override VersionProcessorDTO GetNewItem(IVersionProcessor source)
        {
            try
            {
                GetNewVersionProcessor(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewVersionProcessor(IVersionProcessor source)
        {
            TraceLogger?.AddMessage($"Converting version processor Id={source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            foreach (var item in source.Versions)
            {
                var convertLogic = new DictionaryConvertStrategy<DateVersionDTO, IDateVersion>(this, dataVersionConvertStrategy);
                NewItem.Versions.Add(convertLogic.Convert(item));
            }
            TraceLogger?.AddMessage($"Converting version processor Id={NewItem.Id} has been done successfully", TraceLogStatuses.Service);
        }

        private void InitializeStrategies()
        {
            dataVersionConvertStrategy ??= new DateVersionToDTOConvertStrategy(ReferenceDictionary, TraceLogger);
        }
    }
}
