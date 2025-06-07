using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    public class DateVersionToDTOConvertStrategy : ConvertStrategy<DateVersionDTO, IDateVersion>
    {
        private IUpdateStrategy<IDateVersion> updateStrategy;
        private DictionaryConvertStrategy<ISaveable, ISaveable> convertStrategy;

        public DateVersionToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override DateVersionDTO GetNewItem(IDateVersion source)
        {
            try
            {
                GetNewDateVersion(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewDateVersion(IDateVersion source)
        {
            TraceLogger?.AddMessage("Date version converting is started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new()
            {
                Id = source.Id
            };
            updateStrategy.Update(NewItem, source);
            NewItem.AnalysisVersion = convertStrategy.Convert(source.AnalysisVersion);
            TraceLogger?.AddMessage("Date version converting has been finished", TraceLogStatuses.Service);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new DateVersionUpdateStrategy();
            convertStrategy = new DictionaryConvertStrategy<ISaveable, ISaveable>()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger,
                ConvertStrategy = new VersionItemToDTOConvertStrategy(ReferenceDictionary, TraceLogger)
            };
        }
    }
}
