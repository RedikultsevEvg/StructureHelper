using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class FeaMaterialAnalysisToDTOConvertStrategy : ConvertStrategy<FeaMaterialAnalysisDTO, IFeaMaterialAnalysis>
    {
        private IUpdateStrategy<IFeaMaterialAnalysis> updateStrategy;
        private IConvertStrategy<VersionProcessorDTO, IVersionProcessor> versionProcessorConvertStrategy;
        private IUpdateStrategy<IFeaMaterialAnalysis> UpdateStrategy => updateStrategy ??= new FeaMaterialAnalysisUpdateStrategy() { UpdateChildren = false};

        public FeaMaterialAnalysisToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override FeaMaterialAnalysisDTO GetNewItem(IFeaMaterialAnalysis source)
        {
            CheckObject.ThrowIfNull(source);
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            versionProcessorConvertStrategy = new DictionaryConvertStrategy<VersionProcessorDTO, IVersionProcessor>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = new VersionProcessorToDTOConvertStrategy(ReferenceDictionary, TraceLogger),
                TraceLogger = TraceLogger
            };
            NewItem.VersionProcessor = versionProcessorConvertStrategy.Convert(source.VersionProcessor);
            return NewItem;
        }
    }
}
