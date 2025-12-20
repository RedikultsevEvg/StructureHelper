using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class SteelLibMaterialFromDTOConvertStrategy : ConvertStrategy<SteelLibMaterial, SteelLibMaterialDTO>
    {
        IUpdateStrategy<ISteelLibMaterial> updateStrategy;
        IUpdateStrategy<ISteelLibMaterial> UpdateStrategy => updateStrategy ??= new SteelLibMaterialUpdateStrategy();
        public override SteelLibMaterial GetNewItem(SteelLibMaterialDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
