using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class DateVersionFromDTOConvertStrategy : IConvertStrategy<IDateVersion, IDateVersion>
    {
        private IUpdateStrategy<IDateVersion> updateStrategy;
        private IConvertStrategy<ISaveable, ISaveable> convertStrategy;
        private DictionaryConvertStrategy<ISaveable, ISaveable> convertLogic;

        public DateVersionFromDTOConvertStrategy(IUpdateStrategy<IDateVersion> updateStrategy,
            IConvertStrategy<ISaveable, ISaveable> convertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.convertStrategy = convertStrategy;
        }

        public DateVersionFromDTOConvertStrategy() : this (
            new DateVersionUpdateStrategy(),
            new VersionItemFromDTOConvertStrategy())
        {
            
        }


        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public IDateVersion Convert(IDateVersion source)
        {
            try
            {
                Check();
                return GetDateVersion(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private DateVersion GetDateVersion(IDateVersion source)
        {
            TraceLogger?.AddMessage("Date version converting is started", TraceLogStatuses.Service);
            DateVersion newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            convertLogic = new DictionaryConvertStrategy<ISaveable, ISaveable>(this, convertStrategy);
            newItem.AnalysisVersion = convertLogic.Convert(source.AnalysisVersion);
            TraceLogger?.AddMessage($"Date version date = {newItem.DateTime} converting has been finished", TraceLogStatuses.Service);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<IDateVersion, IDateVersion>(this);
            checkLogic.Check();
        }
    }
}
