using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAccess.DTOs.Converters
{
    public class AnalysisFromDTOConvertStrategy : IConvertStrategy<IAnalysis, IAnalysis>
    {
        private const string Message = "Analysis type is";
        private IConvertStrategy<ICrossSectionNdmAnalysis, ICrossSectionNdmAnalysis> convertCrossSectionNdmAnalysisStrategy = new CrossSectionNdmAnalysisFromDTOConvertStrategy();

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public IAnalysis Convert(IAnalysis source)
        {
            Check();
            try
            {
                IAnalysis analysis = GetAnalysis(source);
                return analysis;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
            
        }

        private IAnalysis GetAnalysis(IAnalysis source)
        {
            IAnalysis analysis;
            if (source is ICrossSectionNdmAnalysis crossSectionNdmAnalysis)
            {
                analysis = GetCrossSectionNdmAnalysis(crossSectionNdmAnalysis);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            foreach (var item in source.VersionProcessor.Versions)
            {
                //to do
            }

            return analysis;
        }

        private ICrossSectionNdmAnalysis GetCrossSectionNdmAnalysis(ICrossSectionNdmAnalysis source)
        {
            TraceLogger?.AddMessage(Message + " Cross-Section Ndm Analysis", TraceLogStatuses.Debug);
            convertCrossSectionNdmAnalysisStrategy.ReferenceDictionary = ReferenceDictionary;
            convertCrossSectionNdmAnalysisStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ICrossSectionNdmAnalysis, ICrossSectionNdmAnalysis>(this, convertCrossSectionNdmAnalysisStrategy);
            ICrossSectionNdmAnalysis crossSectionNdmAnalysis = convertLogic.Convert(source);
            return crossSectionNdmAnalysis;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<IAnalysis, IAnalysis>(this);
            checkLogic.Check();
        }
    }
}
