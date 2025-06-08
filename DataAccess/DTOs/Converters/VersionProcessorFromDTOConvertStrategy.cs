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
    public class VersionProcessorFromDTOConvertStrategy : ConvertStrategy<IVersionProcessor, IVersionProcessor>
    {
        private IConvertStrategy<IDateVersion, IDateVersion> dateVersionConvertStrategy;

        public override IVersionProcessor GetNewItem(IVersionProcessor source)
        {
            ChildClass = this;
            return GetVersionProcessor(source);
        }

        private IVersionProcessor GetVersionProcessor(IVersionProcessor source)
        {
            TraceLogger?.AddMessage("Version processor converting is started", TraceLogStatuses.Debug);
            dateVersionConvertStrategy ??= new DateVersionFromDTOConvertStrategy();
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
    }
}
