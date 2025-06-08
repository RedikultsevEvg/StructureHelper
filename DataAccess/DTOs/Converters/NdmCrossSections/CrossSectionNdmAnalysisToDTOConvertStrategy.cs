using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.Analyses;

namespace DataAccess.DTOs
{
    internal class CrossSectionNdmAnalysisToDTOConvertStrategy : ConvertStrategy<CrossSectionNdmAnalysisDTO, ICrossSectionNdmAnalysis>
    {
        private IUpdateStrategy<ICrossSectionNdmAnalysis> updateStrategy;
        private IConvertStrategy<VersionProcessorDTO, IVersionProcessor> convertStrategy;

        public CrossSectionNdmAnalysisToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override CrossSectionNdmAnalysisDTO GetNewItem(ICrossSectionNdmAnalysis source)
        {
            try
            {
                GetNewAnalysis(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }
        private void GetNewAnalysis(ICrossSectionNdmAnalysis source)
        {
            TraceLogger?.AddMessage("Cross-section ndm analysis converting is started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            TraceLogger?.AddMessage("Convert version processor is started", TraceLogStatuses.Service);
            NewItem.VersionProcessor = convertStrategy.Convert(source.VersionProcessor);
            TraceLogger?.AddMessage("Cross-section ndm analysis has been converted successfully", TraceLogStatuses.Service);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new CrossSectionNdmAnalysisUpdateStrategy();
            convertStrategy = new DictionaryConvertStrategy<VersionProcessorDTO, IVersionProcessor>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = new VersionProcessorToDTOConvertStrategy(ReferenceDictionary, TraceLogger),
                TraceLogger = TraceLogger
            };
        }
    }
}
