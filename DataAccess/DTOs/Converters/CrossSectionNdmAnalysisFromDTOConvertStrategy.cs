using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
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
        private IUpdateStrategy<ICrossSectionNdmAnalysis> updateStrategy = new CrossSectionNdmAnalysisUpdateStrategy();
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ICrossSectionNdmAnalysis Convert(ICrossSectionNdmAnalysis source)
        {
            try
            {
                CrossSectionNdmAnalysis newItem = GetCrossSectinNDMAnalysis(source);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
            
        }

        private CrossSectionNdmAnalysis GetCrossSectinNDMAnalysis(ICrossSectionNdmAnalysis source)
        {
            CrossSectionNdmAnalysis newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
