using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class HasBeamShearActionsToDTOUpdateStrategy : IUpdateStrategy<IHasBeamShearActions>
    {

        private Dictionary<(Guid id, Type type), ISaveable> referenceDictionary;
        private IShiftTraceLogger traceLogger;
        public HasBeamShearActionsToDTOUpdateStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            this.referenceDictionary = referenceDictionary;
            this.traceLogger = traceLogger;
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
                    referenceDictionary,
                    traceLogger,
                    new BeamShearActionToDTOConvertStrategy(referenceDictionary, traceLogger));
                var newAction = convertStrategy.Convert(action);
                targetObject.Actions.Add(newAction);
            }
        }
    }
}
