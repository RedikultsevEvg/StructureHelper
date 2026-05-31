using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class HelperMaterialFromDTOStrategyContainer : IHelperMaterialFromDTOStrategyContainer
    {
        private IConvertStrategy<ConcreteLibMaterial, ConcreteLibMaterialDTO> concreteConvertStrategy;
        private IConvertStrategy<ReinforcementLibMaterial, ReinforcementLibMaterialDTO> reinforcementConvertStrategy;
        private IConvertStrategy<ElasticMaterial, ElasticMaterialDTO> elasticConvertStrategy;
        private IConvertStrategy<FRMaterial, FRMaterialDTO> frConvertStrategy;
        private IUpdateStrategy<IHelperMaterial> safetyFactorUpdateStrategy;
        private IConvertStrategy<SteelLibMaterial, SteelLibMaterialDTO> steelConvertStrategy;
        public IConvertStrategy<UserMaterial, UserMaterialDTO> userMaterialConvertStrategy;
        public IConvertStrategy<ConcreteLibMaterial, ConcreteLibMaterialDTO> ConcreteConvertStrategy => concreteConvertStrategy ??= new ConcreteLibMaterialFromDTOConvertStrategy();
        public IConvertStrategy<ElasticMaterial, ElasticMaterialDTO> ElasticConvertStrategy => elasticConvertStrategy ??= new ElasticMaterialFromDTOConvertStrategy();
        public IConvertStrategy<FRMaterial, FRMaterialDTO> FrConvertStrategy => frConvertStrategy ??= new FRMaterialFromDTOConvertStrategy();
        public IConvertStrategy<ReinforcementLibMaterial, ReinforcementLibMaterialDTO> ReinforcementConvertStrategy => reinforcementConvertStrategy ??= new ReinforcementLibMaterialFromDTOConvertStrategy();
        public IUpdateStrategy<IHelperMaterial> SafetyFactorUpdateStrategy => safetyFactorUpdateStrategy ??= new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorsFromDTOLogic());
        public IConvertStrategy<SteelLibMaterial, SteelLibMaterialDTO> SteelConvertStrategy => steelConvertStrategy ??= new SteelLibMaterialFromDTOConvertStrategy();

        public IConvertStrategy<UserMaterial, UserMaterialDTO> UserMaterialConvertStrategy => userMaterialConvertStrategy ??= new UserMaterialFromDTOConvertStrategy();
    }
}
