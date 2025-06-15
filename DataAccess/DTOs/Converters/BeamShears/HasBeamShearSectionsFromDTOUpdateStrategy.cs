using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class HasBeamShearSectionsFromDTOUpdateStrategy : IUpdateStrategy<IHasBeamShearSections>
    {
        private Dictionary<(Guid id, Type type), ISaveable> referenceDictionary;
        private IShiftTraceLogger traceLogger;
        private IConvertStrategy<BeamShearSection, BeamShearSectionDTO> convertStrategy;

        public HasBeamShearSectionsFromDTOUpdateStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            this.referenceDictionary = referenceDictionary;
            this.traceLogger = traceLogger;
        }

        public void Update(IHasBeamShearSections targetObject, IHasBeamShearSections sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.IsNull(sourceObject.Sections);
            CheckObject.IsNull(targetObject.Sections);
            targetObject.Sections.Clear();
            foreach (var section in sourceObject.Sections)
            {
                targetObject.Sections.Add(ProcessSection(section));
            }
        }

        private IBeamShearSection ProcessSection(IBeamShearSection section)
        {
            if (section is not BeamShearSectionDTO sectionDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(section));
            }
            InitializeStrategies();
            return convertStrategy.Convert(sectionDTO);
        }

        private void InitializeStrategies()
        {
            convertStrategy ??= new DictionaryConvertStrategy<BeamShearSection, BeamShearSectionDTO>
                (referenceDictionary, traceLogger, new BeamShearSectionFromDTOConvertStrategy(referenceDictionary, traceLogger));
        }
    }
}
