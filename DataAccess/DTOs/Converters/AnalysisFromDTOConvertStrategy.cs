using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.Analyses;

namespace DataAccess.DTOs
{
    public class AnalysisFromDTOConvertStrategy : ConvertStrategy<IAnalysis, IAnalysis>
    {
        private const string AnalysisIs = "Analysis type is";

        private IConvertStrategy<IVersionProcessor, IVersionProcessor> versionProcessorConvertStrategy;

        public AnalysisFromDTOConvertStrategy()
        {
        }

        public AnalysisFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override IAnalysis GetNewItem(IAnalysis source)
        {
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
            if (source is ICrossSectionNdmAnalysis crossSectionNdmAnalysis)
            {
                GetCrossSectionNdmAnalysis(crossSectionNdmAnalysis);
            }
            else if (source is IBeamShearAnalysis beamShearAnalysis)
            {
                GetBeamShearAnalysis(beamShearAnalysis);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                throw new StructureHelperException(errorString);
            }
            NewItem.VersionProcessor = GetVersionProcessor(source.VersionProcessor);
            return NewItem;
        }

        private void GetBeamShearAnalysis(IBeamShearAnalysis beamShearAnalysis)
        {
            TraceLogger?.AddMessage(AnalysisIs + " beam shear Analysis", TraceLogStatuses.Debug);
            TraceLogger?.AddMessage("Beam shear analysis converting has been started", TraceLogStatuses.Debug);
            var convertStrategy = new DictionaryConvertStrategy<IBeamShearAnalysis, IBeamShearAnalysis>
                (this, new BeamShearAnalysisFromDTOConvertStrategy(this));
            NewItem = convertStrategy.Convert(beamShearAnalysis);
            TraceLogger?.AddMessage("Beam shear analysis converting has been finished succesfully", TraceLogStatuses.Debug);
        }


        private void GetCrossSectionNdmAnalysis(ICrossSectionNdmAnalysis source)
        {
            TraceLogger?.AddMessage(AnalysisIs + " Cross-Section Ndm Analysis", TraceLogStatuses.Debug);
            TraceLogger?.AddMessage("Cross-Section Ndm Analysis converting has been started", TraceLogStatuses.Debug);
            var convertStrategy = new DictionaryConvertStrategy<ICrossSectionNdmAnalysis, ICrossSectionNdmAnalysis>
                (this, new CrossSectionNdmAnalysisFromDTOConvertStrategy(this));
            NewItem = convertStrategy.Convert(source);
            TraceLogger?.AddMessage("Cross-Section Ndm Analysis converting has been finished successfully", TraceLogStatuses.Debug);
        }
        private IVersionProcessor GetVersionProcessor(IVersionProcessor source)
        {
            TraceLogger?.AddMessage("Version processor converting is started", TraceLogStatuses.Service);
            versionProcessorConvertStrategy ??= new VersionProcessorFromDTOConvertStrategy();
            versionProcessorConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            versionProcessorConvertStrategy.TraceLogger = TraceLogger;
            IVersionProcessor versionProcessor = versionProcessorConvertStrategy.Convert(source);
            TraceLogger?.AddMessage("Version processor converting has been finished successfully", TraceLogStatuses.Service);
            return versionProcessor;
        }

    }
}
