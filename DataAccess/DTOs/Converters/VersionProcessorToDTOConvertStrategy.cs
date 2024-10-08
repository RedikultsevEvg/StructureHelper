using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class VersionProcessorToDTOConvertStrategy : IConvertStrategy<VersionProcessorDTO, IVersionProcessor>
    {
        private IConvertStrategy<DateVersionDTO, IDateVersion> convertStrategy = new DateVersionToDTOConvertStrategy();
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public VersionProcessorDTO Convert(IVersionProcessor source)
        {
            Check();
            VersionProcessorDTO newItem = new()
            {
                Id = source.Id
            };
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            foreach (var item in source.Versions)
            {
                var convertLogic = new DictionaryConvertStrategy<DateVersionDTO, IDateVersion>()
                {
                    ReferenceDictionary = ReferenceDictionary,
                    ConvertStrategy = convertStrategy,
                    TraceLogger = TraceLogger
                };
                newItem.Versions.Add(convertLogic.Convert(item));
            }
            return newItem;
        }
        private void Check()
        {
            var checkLogic = new CheckConvertLogic<VersionProcessorDTO, IVersionProcessor>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
