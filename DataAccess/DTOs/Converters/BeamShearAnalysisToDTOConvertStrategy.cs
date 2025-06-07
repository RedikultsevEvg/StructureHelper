using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;

namespace DataAccess.DTOs
{
    public class BeamShearAnalysisToDTOConvertStrategy : ConvertStrategy<BeamShearAnalysisDTO, IBeamShearAnalysis>
    {
        private IUpdateStrategy<IBeamShearAnalysis> updateStrategy;
        private IConvertStrategy<VersionProcessorDTO, IVersionProcessor> convertStrategy;

        public BeamShearAnalysisToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override BeamShearAnalysisDTO GetNewItem(IBeamShearAnalysis source)
        {
            updateStrategy ??= new BeamShearAnalysisUpdateStrategy();
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
            updateStrategy.Update(NewItem, source);
            NewItem.VersionProcessor = convertStrategy.Convert(source.VersionProcessor);
            TraceLogger?.AddMessage($"Converting beam shear analysis id = {NewItem.Id} has done successfully");
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearAnalysisUpdateStrategy();
            convertStrategy = new DictionaryConvertStrategy<VersionProcessorDTO, IVersionProcessor>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = new VersionProcessorToDTOConvertStrategy(ReferenceDictionary, TraceLogger),
                TraceLogger = TraceLogger
            };
        }
    }
}
