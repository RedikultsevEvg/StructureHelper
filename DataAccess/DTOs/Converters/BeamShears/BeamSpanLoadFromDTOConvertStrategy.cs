using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    internal class BeamSpanLoadFromDTOConvertStrategy : ConvertStrategy<IBeamSpanLoad, IBeamSpanLoad>
    {
        public BeamSpanLoadFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override IBeamSpanLoad GetNewItem(IBeamSpanLoad source)
        {
            CheckObject.ThrowIfNull(source);
            if (source is DistributedLoadDTO distributed)
            {
                ProcessDistributed(distributed);
            }
            else if (source is ConcentratedForceDTO concentrated)
            {
                ProcessConcentrated(concentrated);
            }
            else if (source is TrapezoidDistributedLoadDTO trapezoid)
            {
                ProcessTrapezoid(trapezoid);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source));
            }
            return NewItem;
        }

        private void ProcessTrapezoid(TrapezoidDistributedLoadDTO trapezoid)
        {
            var convertLogic = new DictionaryConvertStrategy<TrapezoidDistributedLoad, TrapezoidDistributedLoadDTO>
                (this, new TrapezoidDistributedLoadFromDTOConvertStrategy(this));
            NewItem = convertLogic.Convert(trapezoid);
        }

        private void ProcessConcentrated(ConcentratedForceDTO concentrated)
        {
            var convertLogic = new DictionaryConvertStrategy<ConcentratedForce, ConcentratedForceDTO>
                (this, new ConcentratedForceFromDTOConvertStrategy(this));
            NewItem = convertLogic.Convert(concentrated);
        }

        private void ProcessDistributed(DistributedLoadDTO distributed)
        {
            var convertLogic = new DictionaryConvertStrategy<DistributedLoad, DistributedLoadDTO>
                (this, new DistributedLoadFromDTOConvertStrategy(this));
            NewItem = convertLogic.Convert(distributed);
        }
    }
}
