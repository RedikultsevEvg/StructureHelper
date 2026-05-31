using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public interface IHelperMaterialFromDTOStrategyContainer
    {
        IConvertStrategy<ConcreteLibMaterial, ConcreteLibMaterialDTO> ConcreteConvertStrategy { get; }
        IConvertStrategy<ElasticMaterial, ElasticMaterialDTO> ElasticConvertStrategy { get; }
        IConvertStrategy<FRMaterial, FRMaterialDTO> FrConvertStrategy { get; }
        IConvertStrategy<ReinforcementLibMaterial, ReinforcementLibMaterialDTO> ReinforcementConvertStrategy { get; }
        IUpdateStrategy<IHelperMaterial> SafetyFactorUpdateStrategy { get; }
        IConvertStrategy<SteelLibMaterial, SteelLibMaterialDTO> SteelConvertStrategy { get; }
        IConvertStrategy<UserMaterial, UserMaterialDTO> UserMaterialConvertStrategy { get; }
    }
}