using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearDesignRangePropertyToDTOConvertStrategy : ConvertStrategy<BeamShearDesignRangePropertyDTO, IBeamShearDesignRangeProperty>
    {
        IUpdateStrategy<IBeamShearDesignRangeProperty>? updateStrategy;

        public BeamShearDesignRangePropertyToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override BeamShearDesignRangePropertyDTO GetNewItem(IBeamShearDesignRangeProperty source)
        {
            updateStrategy ??= new BeamShearDesignRangePropertyUpdateStrategy();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
