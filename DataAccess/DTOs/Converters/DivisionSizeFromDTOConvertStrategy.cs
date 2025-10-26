using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class DivisionSizeFromDTOConvertStrategy : ConvertStrategy<IDivisionSize, IDivisionSize>
    {
        IUpdateStrategy<IDivisionSize> updateStrategy;

        public DivisionSizeFromDTOConvertStrategy(IUpdateStrategy<IDivisionSize> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public DivisionSizeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
            updateStrategy = new DivisionSizeUpdateStrategy();
        }

        public override IDivisionSize GetNewItem(IDivisionSize source)
        {
            if (source is not DivisionSizeDTO sourceDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source));
            }
            NewItem = new DivisionSize(source.Id);
            updateStrategy.Update(NewItem, sourceDTO);
            return NewItem;
        }
    }
}
