using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class FeaMaterialAnalysisFromDTOConvertStrategy : ConvertStrategy<FeaMaterialAnalysis, FeaMaterialAnalysisDTO>
    {
        private IUpdateStrategy<IFeaMaterialAnalysis> updateStrategy;
        private IConvertStrategy<IVersionProcessor, IVersionProcessor> versionProcessorConvertStrategy;

        public FeaMaterialAnalysisFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IFeaMaterialAnalysis> UpdateStrategy => updateStrategy ??= new FeaMaterialAnalysisUpdateStrategy() { UpdateChildren = false };
        public override FeaMaterialAnalysis GetNewItem(FeaMaterialAnalysisDTO source)
        {
            ChildClass = this;
            CheckObject.ThrowIfNull(source);
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            versionProcessorConvertStrategy = new DictionaryConvertStrategy<IVersionProcessor, IVersionProcessor>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = new VersionProcessorFromDTOConvertStrategy(ReferenceDictionary, TraceLogger),
                TraceLogger = TraceLogger
            };
            NewItem.VersionProcessor = versionProcessorConvertStrategy.Convert(source.VersionProcessor);
            return NewItem;
        }
    }
}
