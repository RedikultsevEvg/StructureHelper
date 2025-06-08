using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Analyses;

namespace DataAccess.DTOs
{
    public class BeamShearAnalysisFromDTOConvertStrategy : ConvertStrategy<IBeamShearAnalysis, IBeamShearAnalysis>
    {
        private IUpdateStrategy<IBeamShearAnalysis> updateStrategy;

        public BeamShearAnalysisFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override IBeamShearAnalysis GetNewItem(IBeamShearAnalysis source)
        {
            ChildClass = this;
            GetAnalysis(source);
            return NewItem;
        }

        private void GetAnalysis(IBeamShearAnalysis source)
        {
            updateStrategy ??= new BeamShearAnalysisUpdateStrategy();
            BeamShearAnalysis beamShearAnalysis = new(source.Id);
            updateStrategy.Update(beamShearAnalysis, source);
            NewItem = beamShearAnalysis;
        }
    }
}
