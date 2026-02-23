using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.Analyses;

namespace DataAccess.DTOs
{
    public class AnalysisToDTOConvertStrategy : ConvertStrategy<IAnalysis, IAnalysis>
    {
        private const string Message = "Analysis type is";

        private IConvertStrategy<CrossSectionNdmAnalysisDTO, ICrossSectionNdmAnalysis> crossSectionConvertLogic;
        private IConvertStrategy<BeamShearAnalysisDTO, IBeamShearAnalysis> beamShearConvertLogic;

        public AnalysisToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override IAnalysis GetNewItem(IAnalysis source)
        {
            IAnalysis analysis;
            if (source is ICrossSectionNdmAnalysis crossSectionNdmAnalysis)
            {
                analysis = GetCrossSectionNdmAnalysisDTO(crossSectionNdmAnalysis);
            }
            else if (source is IBeamShearAnalysis beamShearAnalysis)
            {
                analysis = GetBeamShearAnalysis(beamShearAnalysis);
            }
            else if (source is FeaMaterialAnalysis feaMaterialAnalysis)
            {
                analysis = GetFeaMaterialAnalysis(feaMaterialAnalysis);
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

        private FeaMaterialAnalysisDTO GetFeaMaterialAnalysis(FeaMaterialAnalysis feaMaterialAnalysis)
        {
            TraceLogger?.AddMessage(Message + " FEA Material Analysis", TraceLogStatuses.Debug);
            var convertLogic = new FeaMaterialAnalysisToDTOConvertStrategy(this);
            var newItem = convertLogic.Convert(feaMaterialAnalysis);
            return newItem;
        }

        private BeamShearAnalysisDTO GetBeamShearAnalysis(IBeamShearAnalysis beamShearAnalysis)
        {
            TraceLogger?.AddMessage(Message + " Beam Shear Analysis", TraceLogStatuses.Debug);
            beamShearConvertLogic ??= new DictionaryConvertStrategy<BeamShearAnalysisDTO, IBeamShearAnalysis>
                (this,
                new BeamShearAnalysisToDTOConvertStrategy(ReferenceDictionary, TraceLogger)
                );
            BeamShearAnalysisDTO newItem = beamShearConvertLogic.Convert(beamShearAnalysis);
            return newItem;
        }

        private CrossSectionNdmAnalysisDTO GetCrossSectionNdmAnalysisDTO(ICrossSectionNdmAnalysis crossSectionNdmAnalysis)
        {
            TraceLogger?.AddMessage(Message + " Cross-Section Ndm Analysis", TraceLogStatuses.Debug);
            crossSectionConvertLogic ??= new DictionaryConvertStrategy<CrossSectionNdmAnalysisDTO, ICrossSectionNdmAnalysis>
                (this,
                new CrossSectionNdmAnalysisToDTOConvertStrategy(ReferenceDictionary, TraceLogger)
                );
            CrossSectionNdmAnalysisDTO newItem = crossSectionConvertLogic.Convert(crossSectionNdmAnalysis);
            return newItem;
        }
    }
}
