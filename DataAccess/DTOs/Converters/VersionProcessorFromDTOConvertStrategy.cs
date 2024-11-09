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
    public class VersionProcessorFromDTOConvertStrategy : IConvertStrategy<IVersionProcessor, IVersionProcessor>
    {
        private IConvertStrategy<IDateVersion, IDateVersion> dateVersionConvertStrategy;
        private ICheckConvertLogic<IVersionProcessor, IVersionProcessor> checkLogic;

        public VersionProcessorFromDTOConvertStrategy(
            IConvertStrategy<IDateVersion, IDateVersion> dateVersionConvertStrategy)
        {
            this.dateVersionConvertStrategy = dateVersionConvertStrategy;
            this.checkLogic = checkLogic;
        }

        public VersionProcessorFromDTOConvertStrategy() : this(new DateVersionFromDTOConvertStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public IVersionProcessor Convert(IVersionProcessor source)
        {
            try
            {
                Check();
                IVersionProcessor versionProcessor = GetVersionProcessor(source);
                return versionProcessor;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private IVersionProcessor GetVersionProcessor(IVersionProcessor source)
        {
            TraceLogger?.AddMessage("Version processor converting is started", TraceLogStatuses.Debug);
            IVersionProcessor newItem = new VersionProcessor(source.Id);
            TraceLogger?.AddMessage($"Source version processor has {source.Versions.Count} version(s)", TraceLogStatuses.Service);
            dateVersionConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            dateVersionConvertStrategy.TraceLogger = TraceLogger;
            foreach (var item in source.Versions)
            {
                IDateVersion dateVersion = dateVersionConvertStrategy.Convert(item);
                newItem.Versions.Add(dateVersion);
            }
            TraceLogger?.AddMessage($"Totaly {newItem.Versions.Count} version(s) was(were) obtained", TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Version processor has been converted successfully", TraceLogStatuses.Service);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<IVersionProcessor, IVersionProcessor>(this);
            checkLogic.Check();
        }
    }
}
