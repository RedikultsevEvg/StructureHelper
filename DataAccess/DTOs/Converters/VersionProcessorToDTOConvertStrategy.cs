using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class VersionProcessorToDTOConvertStrategy : IConvertStrategy<VersionProcessorDTO, IVersionProcessor>
    {
        private IConvertStrategy<DateVersionDTO, IDateVersion> dataVersionConvertStrategy;
        private ICheckConvertLogic<VersionProcessorDTO, IVersionProcessor> checkLogic;

        public VersionProcessorToDTOConvertStrategy(IConvertStrategy<DateVersionDTO, IDateVersion> dataVersionConvertStrategy)
        {
            this.dataVersionConvertStrategy = dataVersionConvertStrategy;
        }
        public VersionProcessorToDTOConvertStrategy() : this( new DateVersionToDTOConvertStrategy())
        {

        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }


        public VersionProcessorDTO Convert(IVersionProcessor source)
        {
            Check();
            try
            {
                VersionProcessorDTO versionProcessorDTO = GetNewVersionProcessor(source);
                return versionProcessorDTO;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }   
        }

        private VersionProcessorDTO GetNewVersionProcessor(IVersionProcessor source)
        {
            VersionProcessorDTO newItem = new()
            {
                Id = source.Id
            };
            dataVersionConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            dataVersionConvertStrategy.TraceLogger = TraceLogger;
            foreach (var item in source.Versions)
            {
                var convertLogic = new DictionaryConvertStrategy<DateVersionDTO, IDateVersion>(this, dataVersionConvertStrategy);
                newItem.Versions.Add(convertLogic.Convert(item));
            }
            return newItem;
        }

        private void Check()
        {
            checkLogic = new CheckConvertLogic<VersionProcessorDTO, IVersionProcessor>(this);
            checkLogic.Check();
        }
    }
}
