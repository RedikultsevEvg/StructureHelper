using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class HasBeamShearActionToDTOConvertStrategy : IUpdateStrategy<IHasBeamShearActions>
    {

        private Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get;}
        private IShiftTraceLogger TraceLogger { get;}
        public HasBeamShearActionToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            ReferenceDictionary = referenceDictionary;
            TraceLogger = traceLogger;
        }

        public void Update(IHasBeamShearActions targetObject, IHasBeamShearActions sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.IsNull(sourceObject.Actions);
            CheckObject.IsNull(targetObject.Actions);
            targetObject.Actions.Clear();
            foreach (var action in sourceObject.Actions)
            {
                var convertStrategy = new DictionaryConvertStrategy<BeamShearActionDTO, IBeamShearAction>(
                    ReferenceDictionary,
                    TraceLogger,
                    new BeamShearActionConvertStrategy(ReferenceDictionary, TraceLogger));
                var newAction = convertStrategy.Convert(action);
                targetObject.Actions.Add(newAction);
            }
        }
    }
}
