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
    public class CrossSectionNdmAnalysisFromDTOConvertStrategy : IConvertStrategy<ICrossSectionNdmAnalysis, ICrossSectionNdmAnalysis>
    {
        private IUpdateStrategy<ICrossSectionNdmAnalysis> updateStrategy;

        public CrossSectionNdmAnalysisFromDTOConvertStrategy(IUpdateStrategy<ICrossSectionNdmAnalysis> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public CrossSectionNdmAnalysisFromDTOConvertStrategy() : this(new CrossSectionNdmAnalysisUpdateStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ICrossSectionNdmAnalysis Convert(ICrossSectionNdmAnalysis source)
        {
            try
            {
                Check();
                ICrossSectionNdmAnalysis newItem = GetCrossSectionNDMAnalysis(source);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
            
        }

        private ICrossSectionNdmAnalysis GetCrossSectionNDMAnalysis(ICrossSectionNdmAnalysis source)
        {
            TraceLogger?.AddMessage("Cross-section sonverting is started");
            CrossSectionNdmAnalysis newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            TraceLogger?.AddMessage("Cross-section analysis was obtained successfully");
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<ICrossSectionNdmAnalysis, ICrossSectionNdmAnalysis>(this);
            checkLogic.Check();
        }
    }
}
