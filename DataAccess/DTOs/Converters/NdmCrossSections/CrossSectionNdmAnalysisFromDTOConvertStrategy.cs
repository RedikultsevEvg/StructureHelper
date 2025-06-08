using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.Analyses;

namespace DataAccess.DTOs.Converters
{
    public class CrossSectionNdmAnalysisFromDTOConvertStrategy : ConvertStrategy<ICrossSectionNdmAnalysis, ICrossSectionNdmAnalysis>
    {
        private IUpdateStrategy<ICrossSectionNdmAnalysis> updateStrategy;

        public CrossSectionNdmAnalysisFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ICrossSectionNdmAnalysis GetNewItem(ICrossSectionNdmAnalysis source)
        {
            ChildClass = this;
            GetAnalysis(source);
            return NewItem;
        }

        private void GetAnalysis(ICrossSectionNdmAnalysis source)
        {
            TraceLogger?.AddMessage("Cross-section analysis converting has been started");
            updateStrategy ??= new CrossSectionNdmAnalysisUpdateStrategy();
            NewItem = new CrossSectionNdmAnalysis(source.Id);
            updateStrategy.Update(NewItem, source);
            TraceLogger?.AddMessage("Cross-section analysis has been finished successfully");
        }

    }
}
