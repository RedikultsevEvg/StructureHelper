using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class HasBeamShearSectionsToDTORenameStrategy : IUpdateStrategy<IHasBeamShearSections>
    {
        private IConvertStrategy<BeamShearSectionDTO, IBeamShearSection> convertStrategy;
        private Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; }
        private IShiftTraceLogger TraceLogger { get; }

        public HasBeamShearSectionsToDTORenameStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            ReferenceDictionary = referenceDictionary;
            TraceLogger = traceLogger;
        }

        public void Update(IHasBeamShearSections targetObject, IHasBeamShearSections sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.IsNull(sourceObject.Sections);
            CheckObject.IsNull(targetObject.Sections);
            InitializeStrategies();
            targetObject.Sections.Clear();
            foreach (var section in sourceObject.Sections)
            {
                var newSection = convertStrategy.Convert(section);
                targetObject.Sections.Add(newSection);
            }
        }

        private void InitializeStrategies()
        {
            convertStrategy = new DictionaryConvertStrategy<BeamShearSectionDTO, IBeamShearSection>
                (ReferenceDictionary, TraceLogger, new BeamShearSectionToDTOConvertStrategy(ReferenceDictionary, TraceLogger));
        }
    }
}
