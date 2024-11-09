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
        private const string AnalysisIs = "Analysis type is";

        private IConvertStrategy<ICrossSectionNdmAnalysis, ICrossSectionNdmAnalysis> convertCrossSectionNdmAnalysisStrategy;
        private IConvertStrategy<IVersionProcessor, IVersionProcessor> versionProcessorConvertStrategy;

        public AnalysisFromDTOConvertStrategy(IConvertStrategy<ICrossSectionNdmAnalysis, ICrossSectionNdmAnalysis> convertCrossSectionNdmAnalysisStrategy,
            IConvertStrategy<IVersionProcessor, IVersionProcessor> versionProcessorConvertStrategy)
        {
            this.convertCrossSectionNdmAnalysisStrategy = convertCrossSectionNdmAnalysisStrategy;
            this.versionProcessorConvertStrategy = versionProcessorConvertStrategy;
        }

        public AnalysisFromDTOConvertStrategy() : this (new CrossSectionNdmAnalysisFromDTOConvertStrategy(),
            new VersionProcessorFromDTOConvertStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public IAnalysis Convert(IAnalysis source)
        {
            try
            {
                Check();
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
            IAnalysis newItem;
            if (source is ICrossSectionNdmAnalysis crossSectionNdmAnalysis)
            {
                newItem = GetCrossSectionNdmAnalysis(crossSectionNdmAnalysis);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            newItem.VersionProcessor = GetVersionProcessor(source.VersionProcessor);
            return newItem;
        }

        private IVersionProcessor GetVersionProcessor(IVersionProcessor source)
        {
            TraceLogger?.AddMessage("Version processor converting is started", TraceLogStatuses.Service);
            versionProcessorConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            versionProcessorConvertStrategy.TraceLogger = TraceLogger;
            IVersionProcessor versionProcessor = versionProcessorConvertStrategy.Convert(source);
            TraceLogger?.AddMessage("Version processor converting has been finished successfully", TraceLogStatuses.Service);
            return versionProcessor;
        }

        private ICrossSectionNdmAnalysis GetCrossSectionNdmAnalysis(ICrossSectionNdmAnalysis source)
        {
            TraceLogger?.AddMessage(AnalysisIs + " Cross-Section Ndm Analysis", TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Cross-Section Ndm Analysis converting is started", TraceLogStatuses.Service);
            convertCrossSectionNdmAnalysisStrategy.ReferenceDictionary = ReferenceDictionary;
            convertCrossSectionNdmAnalysisStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ICrossSectionNdmAnalysis, ICrossSectionNdmAnalysis>(this, convertCrossSectionNdmAnalysisStrategy);
            ICrossSectionNdmAnalysis crossSectionNdmAnalysis = convertLogic.Convert(source);
            TraceLogger?.AddMessage("Cross-Section Ndm Analysis converting has been finished successfully", TraceLogStatuses.Service);
            return crossSectionNdmAnalysis;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<IAnalysis, IAnalysis>(this);
            checkLogic.Check();
        }
    }
}
