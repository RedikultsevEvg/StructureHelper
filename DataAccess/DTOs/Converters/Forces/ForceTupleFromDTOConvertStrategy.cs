using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class ForceTupleFromDTOConvertStrategy : ConvertStrategy<ForceTuple, ForceTupleDTO>
    {
        private IUpdateStrategy<IForceTuple> updateStrategy;

        public ForceTupleFromDTOConvertStrategy()
        {
        }

        public ForceTupleFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override ForceTuple GetNewItem(ForceTupleDTO source)
        {
            updateStrategy ??= new ForceTupleUpdateStrategy();
            ForceTuple newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
