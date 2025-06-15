using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    internal class StirrupByDensityFromDTOConvertStrategy : ConvertStrategy<StirrupByDensity, StirrupByDensityDTO>
    {
        private IUpdateStrategy<IStirrupByDensity> updateStrategy;

        public StirrupByDensityFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override StirrupByDensity GetNewItem(StirrupByDensityDTO source)
        {
            updateStrategy ??= new StirrupByDensityUpdateStrategy();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
