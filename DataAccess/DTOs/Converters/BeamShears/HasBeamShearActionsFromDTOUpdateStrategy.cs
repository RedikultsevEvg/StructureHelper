using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class HasBeamShearActionsFromDTOUpdateStrategy : IUpdateStrategy<IHasBeamShearActions>
    {
        private Dictionary<(Guid id, Type type), ISaveable> referenceDictionary;
        private IShiftTraceLogger traceLogger;
        private IConvertStrategy<BeamShearAction, BeamShearActionDTO> convertStrategy;
        
        public HasBeamShearActionsFromDTOUpdateStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
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
                targetObject.Actions.Add(ProcessAction(action));

            }
        }

        private IBeamShearAction ProcessAction(IBeamShearAction action)
        {
            if (action is not BeamShearActionDTO actionDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(action));
            }
            InitializeStrategies();
            var newAction = convertStrategy.Convert(actionDTO);
            return newAction;
        }

        private void InitializeStrategies()
        {
            convertStrategy ??= new DictionaryConvertStrategy<BeamShearAction, BeamShearActionDTO>(
            referenceDictionary, traceLogger,
            new BeamShearActionFromDTOConvertStrategy(referenceDictionary, traceLogger));
        }
    }
}
