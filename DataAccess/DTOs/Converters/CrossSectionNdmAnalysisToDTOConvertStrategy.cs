using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
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

        public CrossSectionNdmAnalysisToDTOConvertStrategy() : this(new CrossSectionNdmAnalysisUpdateStrategy(),
            new VersionProcessorToDTOConvertStrategy(),
            null)
        {
            
        }

        public CrossSectionNdmAnalysisDTO Convert(ICrossSectionNdmAnalysis source)
        {
            Check();
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
            newItem.VersionProcessor = convertLogic.Convert(source.VersionProcessor);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<CrossSectionNdmAnalysisDTO, ICrossSectionNdmAnalysis>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
