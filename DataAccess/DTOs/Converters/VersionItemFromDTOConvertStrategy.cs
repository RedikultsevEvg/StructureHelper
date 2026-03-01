using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.CrossSections;

namespace DataAccess.DTOs
{
    public class VersionItemFromDTOConvertStrategy : ConvertStrategy<ISaveable, ISaveable>
    {
        private const string AnalysisIs = "Analysis type is";
        private IConvertStrategy<ICrossSection, ICrossSection> crossSectionConvertStrategy;

        public VersionItemFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ISaveable GetNewItem(ISaveable source)
        {
            ChildClass = this;
            return GetNewAnalysis(source);
        }

        private ISaveable GetNewAnalysis(ISaveable source)
        {
            ISaveable newItem;
            if (source is ICrossSection crossSection)
            {
                newItem = ProcessCrossSection(crossSection);
            }
            else if (source is BeamShearDTO beamShear)
            {
                newItem = ProcessBeamShear(beamShear);
            }
            else if (source is FeaMaterialRepositoryDTO feaMaterial)
                newItem = ProcessFeaMaterialsRepository(feaMaterial);
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            TraceLogger?.AddMessage($"Object of type <<{newItem.GetType()}>> was obtained successfully", TraceLogStatuses.Service);
            return newItem;
        }

        private ISaveable ProcessFeaMaterialsRepository(FeaMaterialRepositoryDTO source)
        {
            TraceLogger?.AddMessage(AnalysisIs + " FEA material analysis", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"FEA material analysis Id = {source.Id} converting has been started", TraceLogStatuses.Service);
            var convertLogic = new DictionaryConvertStrategy<FeaMaterialRepository, FeaMaterialRepositoryDTO>
                (this,
                new FeaMaterialRepositoryFromDTOConvertStrategy(this));
            FeaMaterialRepository newItem = convertLogic.Convert(source);
            TraceLogger?.AddMessage($"Fea material analysis Id = {newItem.Id} converting has been finished successfully", TraceLogStatuses.Service);
            return newItem;
        }

        private IBeamShear ProcessBeamShear(BeamShearDTO source)
        {
            TraceLogger?.AddMessage(AnalysisIs + " Beam shear", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Beam shear analysis Id = {source.Id} converting has been started", TraceLogStatuses.Service);
            var convertLogic = new DictionaryConvertStrategy<BeamShear, BeamShearDTO>
                (this,
                new BeamShearFromDTOConvertStrategy(this));
            IBeamShear newItem = convertLogic.Convert(source);
            TraceLogger?.AddMessage($"Beam shear analysis Id = {newItem.Id} converting has been finished successfully", TraceLogStatuses.Service);
            return newItem;
        }

        private ICrossSection ProcessCrossSection(ICrossSection source)
        {
            TraceLogger?.AddMessage(AnalysisIs + " Cross-Section", TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Cross-Section converting has been started", TraceLogStatuses.Service);
            crossSectionConvertStrategy ??= new CrossSectionFromDTOConvertStrategy(ReferenceDictionary, TraceLogger);
            var convertLogic = new DictionaryConvertStrategy<ICrossSection, ICrossSection>(this, crossSectionConvertStrategy);
            ICrossSection newItem = convertLogic.Convert(source);
            TraceLogger?.AddMessage("Cross-Section converting has been finished successfully", TraceLogStatuses.Service);
            return newItem;
        }
    }
}
