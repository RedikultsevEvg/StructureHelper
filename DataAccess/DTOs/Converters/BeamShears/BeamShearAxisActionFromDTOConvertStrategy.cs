using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    internal class BeamShearAxisActionFromDTOConvertStrategy : ConvertStrategy<BeamShearAxisAction, BeamShearAxisActionDTO>
    {
        private IUpdateStrategy<IBeamShearAxisAction> updateStrategy;
        private IConvertStrategy<FactoredForceTuple, FactoredForceTupleDTO> factoredTupleConvertStrategy;
        private IConvertStrategy<IBeamSpanLoad, IBeamSpanLoad> spanLoadConvertLogic;

        public BeamShearAxisActionFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override BeamShearAxisAction GetNewItem(BeamShearAxisActionDTO source)
        {
            CheckObject.ThrowIfNull(source);
            CheckObject.ThrowIfNull(source.SupportForce);
            CheckObject.ThrowIfNull(source.ShearLoads);
            InitializeStrategies();
            GetNewAction(source);
            return NewItem;
        }

        private void GetNewAction(BeamShearAxisActionDTO source)
        {
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            if (source.SupportForce is not FactoredForceTupleDTO factoredForceTupleDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.SupportForce));
            }
            NewItem.SupportForce = factoredTupleConvertStrategy.Convert(factoredForceTupleDTO);
            List<IBeamSpanLoad> beamSpanLoads = GetSpanLoads(source.ShearLoads);
            NewItem.ShearLoads.Clear();
            NewItem.ShearLoads.AddRange(beamSpanLoads);
        }

        private List<IBeamSpanLoad> GetSpanLoads(IEnumerable<IBeamSpanLoad> shearLoads)
        {
            List<IBeamSpanLoad> beamSpanLoads = new();
            foreach (var spanLoad in shearLoads)
            {
                beamSpanLoads.Add(ProcessSpanLoad(spanLoad));
            }
            return beamSpanLoads;
        }

        private IBeamSpanLoad ProcessSpanLoad(IBeamSpanLoad spanLoad)
        {
            return spanLoadConvertLogic.Convert(spanLoad);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearAxisActionUpdateStrategy();
            factoredTupleConvertStrategy ??= new DictionaryConvertStrategy<FactoredForceTuple, FactoredForceTupleDTO>
                (this, new FactoredForceTupleFromDTOConvertStrategy(this));
            spanLoadConvertLogic ??= new DictionaryConvertStrategy<IBeamSpanLoad, IBeamSpanLoad>
                (this, new BeamSpanLoadFromDTOConvertStrategy(this));
        }
    }
}
