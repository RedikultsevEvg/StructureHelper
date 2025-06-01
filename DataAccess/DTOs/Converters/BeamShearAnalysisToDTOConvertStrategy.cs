using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.Analyses;

namespace DataAccess.DTOs
{
    public class BeamShearAnalysisToDTOConvertStrategy : ConvertStrategy<BeamShearAnalysisDTO, IBeamShearAnalysis>
    {
        private IUpdateStrategy<IBeamShearAnalysis> updateStrategy;

        public BeamShearAnalysisToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override BeamShearAnalysisDTO GetNewItem(IBeamShearAnalysis source)
        {
            updateStrategy ??= new BeamShearAnalysisUpdateStrategy();
            try
            {
                NewItem = new(source.Id);
                updateStrategy.Update(NewItem, source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }
    }
}
