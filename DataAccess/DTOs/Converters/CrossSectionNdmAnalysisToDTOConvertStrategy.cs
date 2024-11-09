using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    internal class CrossSectionNdmAnalysisToDTOConvertStrategy : IConvertStrategy<CrossSectionNdmAnalysisDTO, ICrossSectionNdmAnalysis>
    {
        private IUpdateStrategy<ICrossSectionNdmAnalysis> updateStrategy;
        private IConvertStrategy<VersionProcessorDTO, IVersionProcessor> convertStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public CrossSectionNdmAnalysisToDTOConvertStrategy(
            IUpdateStrategy<ICrossSectionNdmAnalysis> updateStrategy,
            IConvertStrategy<VersionProcessorDTO, IVersionProcessor> convertStrategy,
            IShiftTraceLogger traceLogger)
        {
            this.updateStrategy = updateStrategy;
            this.convertStrategy = convertStrategy;
            this.TraceLogger = traceLogger;
        }

        public CrossSectionNdmAnalysisToDTOConvertStrategy() : this(
            new CrossSectionNdmAnalysisUpdateStrategy(),
            new VersionProcessorToDTOConvertStrategy(),
            null)
        {
            
        }

        public CrossSectionNdmAnalysisDTO Convert(ICrossSectionNdmAnalysis source)
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

        private CrossSectionNdmAnalysisDTO GetNewAnalysis(ICrossSectionNdmAnalysis source)
        {
            TraceLogger?.AddMessage("Cross-section ndm analysis converting is started", TraceLogStatuses.Debug);
            CrossSectionNdmAnalysisDTO newItem = new();
            newItem.Id = source.Id;
            updateStrategy.Update(newItem, source);
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<VersionProcessorDTO, IVersionProcessor>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = convertStrategy,
                TraceLogger = TraceLogger
            };
            TraceLogger?.AddMessage("Convert version processor is started", TraceLogStatuses.Service);
            newItem.VersionProcessor = convertLogic.Convert(source.VersionProcessor);
            TraceLogger?.AddMessage("Cross-section ndm analysis has been converted successfully", TraceLogStatuses.Service);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<CrossSectionNdmAnalysisDTO, ICrossSectionNdmAnalysis>(this);
            checkLogic.Check();
        }
    }
}
