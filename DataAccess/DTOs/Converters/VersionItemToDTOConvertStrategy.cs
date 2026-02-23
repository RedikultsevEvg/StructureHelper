using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.CrossSections;

namespace DataAccess.DTOs
{
    public class VersionItemToDTOConvertStrategy : ConvertStrategy<ISaveable, ISaveable>
    {
        private const string AnalysisIs = "Analysis type is";

        public VersionItemToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        private ISaveable GetNewAnalysis(ISaveable source)
        {
            ISaveable newItem;
            if (source is ICrossSection crossSection)
            {
                newItem = ProcessCrossSection(crossSection);
            }
            else if (source is IBeamShear beamShear)
            {
                newItem = ProcessBeamShear(beamShear);
            }
            else if (source is IFeaMaterialRepository materialRepository)
            {
                newItem = ProcessFeaMaterialRepository(materialRepository);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            return newItem;
        }

        private ISaveable ProcessFeaMaterialRepository(IFeaMaterialRepository materialRepository)
        {
            TraceLogger?.AddMessage(AnalysisIs + " Fea material repository", TraceLogStatuses.Debug);
            var convertLogic = new DictionaryConvertStrategy<FeaMaterialRepositoryDTO, IFeaMaterialRepository>()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger,
                ConvertStrategy = new FeaMaterialRepositoryToDTOConvertStrategy(this)
            };
            return convertLogic.Convert(materialRepository);
        }

        private BeamShearDTO ProcessBeamShear(IBeamShear beamShear)
        {
            TraceLogger?.AddMessage(AnalysisIs + " Beam Shear Analysis", TraceLogStatuses.Debug);
            var convertLogic = new DictionaryConvertStrategy<BeamShearDTO, IBeamShear>()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger,
                ConvertStrategy = new BeamShearToDTOConvertStrategy(this)
            };
            return convertLogic.Convert(beamShear);
        }

        private ISaveable ProcessCrossSection(ICrossSection crossSection)
        {
            TraceLogger?.AddMessage(AnalysisIs + " Cross-Section Ndm Analysis", TraceLogStatuses.Debug);
            ISaveable saveable;
            IConvertStrategy<CrossSectionDTO, ICrossSection> crossSectionConvertStrategy = new CrossSectionToDTOConvertStrategy
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            var convertLogic = new DictionaryConvertStrategy<CrossSectionDTO, ICrossSection>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = crossSectionConvertStrategy,
                TraceLogger = TraceLogger
            };
            saveable = convertLogic.Convert(crossSection);
            return saveable;
        }

        public override ISaveable GetNewItem(ISaveable source)
        {
            try
            {
                return GetNewAnalysis(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }
    }
}
