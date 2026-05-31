using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public interface IHelperMaterialToDTOStrategyContainer
    {
        LibMaterialToDTOConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial> ConcreteConvertStrategy { get; }
        IConvertStrategy<ElasticMaterialDTO, IElasticMaterial> ElasticConvertStrategy { get; }
        IConvertStrategy<FRMaterialDTO, IFRMaterial> FrMaterialConvertStrategy { get; }
        LibMaterialToDTOConvertStrategy<ReinforcementLibMaterialDTO, IReinforcementLibMaterial> ReinforcementConvertStrategy { get; }
        IUpdateStrategy<IHelperMaterial> SafetyFactorUpdateStrategy { get; }
        LibMaterialToDTOConvertStrategy<SteelLibMaterialDTO, ISteelLibMaterial> SteelConvertStrategy { get; }
        IConvertStrategy<UserMaterialDTO, IUserMaterial> UserMaterialConvertStrategy { get; }
    }
}