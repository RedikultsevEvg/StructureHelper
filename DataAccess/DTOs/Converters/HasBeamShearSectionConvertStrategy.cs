using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class HasBeamShearSectionConvertStrategy : IUpdateStrategy<IHasBeamShearSections>
    {
        private Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; }
        private IShiftTraceLogger TraceLogger { get; }
        public HasBeamShearSectionConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
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
            targetObject.Sections.Clear();
        }
    }
}
