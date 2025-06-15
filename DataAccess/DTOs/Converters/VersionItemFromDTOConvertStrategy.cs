using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
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
            else if (source is IBeamShear beamShear)
            {
                newItem = ProcessBeamShear(beamShear);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            TraceLogger?.AddMessage($"Object of type <<{newItem.GetType()}>> was obtained successfully", TraceLogStatuses.Service);
            return newItem;
        }

        private IBeamShear ProcessBeamShear(IBeamShear source)
        {
            if (source is not BeamShearDTO beamShearDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source));
            }
            TraceLogger?.AddMessage(AnalysisIs + " Beam shear", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Beam shear analysis Id = {source.Id} converting has been started", TraceLogStatuses.Service);
            var convertLogic = new DictionaryConvertStrategy<BeamShear, BeamShearDTO>
                (this,
                new BeamShearFromDTOConvertStrategy(this));
            IBeamShear newItem = convertLogic.Convert(beamShearDTO);
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
