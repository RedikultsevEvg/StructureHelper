using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class SteelLibMaterialToDTOConvertStrategy : LibMaterialToDTOConvertStrategy<SteelLibMaterialDTO, ISteelLibMaterial>
    {
        private IUpdateStrategy<ISteelLibMaterial> updateStrategy;
        public override IUpdateStrategy<ISteelLibMaterial> UpdateStrategy => updateStrategy ??= new SteelLibMaterialUpdateStrategy();

        public override SteelLibMaterialDTO GetMaterialDTO(ISteelLibMaterial source)
        {
            SteelLibMaterialDTO material = new(source.Id);
            return material;
        }
    }
}
