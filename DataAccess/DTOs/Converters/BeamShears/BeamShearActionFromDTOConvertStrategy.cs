using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class BeamShearActionFromDTOConvertStrategy : ConvertStrategy<BeamShearAction, BeamShearActionDTO>
    {
        private IUpdateStrategy<IBeamShearAction> updateStrategy;
        private IConvertStrategy<FactoredForceTuple, FactoredForceTupleDTO> factoredTupleConvertStrategy;
        private IConvertStrategy<BeamShearAxisAction, BeamShearAxisActionDTO> axisActionConvertStrategy;

        public BeamShearActionFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override BeamShearAction GetNewItem(BeamShearActionDTO source)
        {
            ChildClass = this;
            CheckObjects(source);
            InitializeStrategies();
            GetNewAction(source);
            return NewItem;
        }

        private void CheckObjects(BeamShearActionDTO source)
        {
            CheckObject.ThrowIfNull(source);
            CheckObject.ThrowIfNull(source.ExternalForce);
            CheckObject.ThrowIfNull(source.SupportAction);
        }

        private void GetNewAction(BeamShearActionDTO source)
        {
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            if (source.ExternalForce is not FactoredForceTupleDTO forceTupleDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.ExternalForce));
            }
            NewItem.ExternalForce = factoredTupleConvertStrategy.Convert(forceTupleDTO);
            if (source.SupportAction is not BeamShearAxisActionDTO axisActionDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.ExternalForce));
            }
            NewItem.SupportAction = axisActionConvertStrategy.Convert(axisActionDTO);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearActionUpdateStrategy();
            factoredTupleConvertStrategy ??= new DictionaryConvertStrategy<FactoredForceTuple, FactoredForceTupleDTO>
                (this, new FactoredForceTupleFromDTOConvertStrategy(this));
            axisActionConvertStrategy ??= new DictionaryConvertStrategy<BeamShearAxisAction, BeamShearAxisActionDTO>
                (this, new BeamShearAxisActionFromDTOConvertStrategy(this));
        }
    }
}
