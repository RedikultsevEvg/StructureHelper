using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class DateVersionToDTOConvertStrategy : IConvertStrategy<DateVersionDTO, IDateVersion>
    {
        private IUpdateStrategy<IDateVersion> updateStrategy;
        private IConvertStrategy<ISaveable, ISaveable> convertStrategy;

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public DateVersionToDTOConvertStrategy(
            IUpdateStrategy<IDateVersion> updateStrategy,
            IConvertStrategy<ISaveable, ISaveable> convertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.convertStrategy = convertStrategy;
        }

        public DateVersionToDTOConvertStrategy() : this (
            new DateVersionUpdateStrategy(),
            new VersionItemToDTOConvertStrategy())
        {
            
        }

        public DateVersionDTO Convert(IDateVersion source)
        {
            Check();
            DateVersionDTO newItem = new()
            {
                Id = source.Id
            };
            updateStrategy.Update(newItem, source);
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ISaveable, ISaveable>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = convertStrategy,
                TraceLogger = TraceLogger
            };
            newItem.AnalysisVersion = convertLogic.Convert(source.AnalysisVersion);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<DateVersionDTO, IDateVersion>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
