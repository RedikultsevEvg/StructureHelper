using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs.Converters.BeamShears
{
    public class BeamShearDesignRangePropertyFromDTOConvertStrategy : ConvertStrategy<BeamShearDesignRangeProperty, BeamShearDesignRangePropertyDTO>
    {
        private IUpdateStrategy<IBeamShearDesignRangeProperty> updateStrategy;

        public BeamShearDesignRangePropertyFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override BeamShearDesignRangeProperty GetNewItem(BeamShearDesignRangePropertyDTO source)
        {
            updateStrategy ??= new BeamShearDesignRangePropertyUpdateStrategy();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
