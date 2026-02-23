using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.Analyses;

namespace DataAccess.DTOs
{
    public class BeamShearAnalysisToDTOConvertStrategy : ConvertStrategy<BeamShearAnalysisDTO, IBeamShearAnalysis>
    {
        private IUpdateStrategy<IBeamShearAnalysis> updateStrategy;
        private IConvertStrategy<VersionProcessorDTO, IVersionProcessor> versionProcessorConvertStrategy;
        private IUpdateStrategy<IBeamShearAnalysis> UpdateStrategy => updateStrategy ??= new BeamShearAnalysisUpdateStrategy();

        public BeamShearAnalysisToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override BeamShearAnalysisDTO GetNewItem(IBeamShearAnalysis source)
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

        private void GetNewAnalysis(IBeamShearAnalysis source)
        {
            TraceLogger?.AddMessage($"Converting beam shear analysis id = {source.Id} has been started");
            InitializeStrategies();
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            NewItem.VersionProcessor = versionProcessorConvertStrategy.Convert(source.VersionProcessor);
            TraceLogger?.AddMessage($"Converting beam shear analysis id = {NewItem.Id} has done successfully");
        }

        private void InitializeStrategies()
        {
            versionProcessorConvertStrategy = new DictionaryConvertStrategy<VersionProcessorDTO, IVersionProcessor>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = new VersionProcessorToDTOConvertStrategy(ReferenceDictionary, TraceLogger),
                TraceLogger = TraceLogger
            };
        }
    }
}
