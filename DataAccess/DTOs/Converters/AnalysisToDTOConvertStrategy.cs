using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogic.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace DataAccess.DTOs.Converters
{
    public class AnalysisToDTOConvertStrategy : IConvertStrategy<IAnalysis, IAnalysis>
    {
        private const string Message = "Analysis type is";
        private IConvertStrategy<CrossSectionNdmAnalysisDTO, ICrossSectionNdmAnalysis> convertCrossSectionNdmAnalysisStrategy = new CrossSectionNdmAnalysisToDTOConvertStrategy();
        private DictionaryConvertStrategy<CrossSectionNdmAnalysisDTO, ICrossSectionNdmAnalysis> convertLogic;

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public IAnalysis Convert(IAnalysis source)
        {
            Check();
            IAnalysis analysis;
            if (source is ICrossSectionNdmAnalysis crossSectionNdmAnalysis)
            {
                analysis = GetCrossSectionNdmAnalysisDTO(crossSectionNdmAnalysis);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            foreach (var item in source.VersionProcessor.Versions)
            {
                
            }
            return analysis;
        }

        private CrossSectionNdmAnalysisDTO GetCrossSectionNdmAnalysisDTO(ICrossSectionNdmAnalysis crossSectionNdmAnalysis)
        {
            TraceLogger?.AddMessage(Message + " Cross-Section Ndm Analysis", TraceLogStatuses.Debug);
            convertCrossSectionNdmAnalysisStrategy.ReferenceDictionary = ReferenceDictionary;
            convertCrossSectionNdmAnalysisStrategy.TraceLogger = TraceLogger;
            convertLogic = new DictionaryConvertStrategy<CrossSectionNdmAnalysisDTO, ICrossSectionNdmAnalysis>(this, convertCrossSectionNdmAnalysisStrategy);
            CrossSectionNdmAnalysisDTO crossSectionNdmAnalysisDTO = convertLogic.Convert(crossSectionNdmAnalysis);
            return crossSectionNdmAnalysisDTO;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<IAnalysis, IAnalysis>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
