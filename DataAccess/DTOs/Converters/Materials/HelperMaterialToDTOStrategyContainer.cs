using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class HelperMaterialToDTOStrategyContainer : IHelperMaterialToDTOStrategyContainer
    {
        private LibMaterialToDTOConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial> concreteConvertStrategy;
        private LibMaterialToDTOConvertStrategy<ReinforcementLibMaterialDTO, IReinforcementLibMaterial> reinforcementConvertStrategy;
        private LibMaterialToDTOConvertStrategy<SteelLibMaterialDTO, ISteelLibMaterial> steelConvertStrategy;
        private IConvertStrategy<ElasticMaterialDTO, IElasticMaterial> elasticConvertStrategy;
        private IConvertStrategy<FRMaterialDTO, IFRMaterial> frMaterialConvertStrategy;
        private IUpdateStrategy<IHelperMaterial> safetyFactorUpdateStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorToDTOLogic());
        public LibMaterialToDTOConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial> ConcreteConvertStrategy => concreteConvertStrategy ??= new ConcreteLibMaterialToDTOConvertStrategy();
        public IConvertStrategy<ElasticMaterialDTO, IElasticMaterial> ElasticConvertStrategy => elasticConvertStrategy ??= new ElasticMaterialToDTOConvertStrategy();
        public IConvertStrategy<FRMaterialDTO, IFRMaterial> FrMaterialConvertStrategy => frMaterialConvertStrategy ??= new FRMaterialToDTOConvertStrategy();
        public LibMaterialToDTOConvertStrategy<ReinforcementLibMaterialDTO, IReinforcementLibMaterial> ReinforcementConvertStrategy => reinforcementConvertStrategy ??= new ReinforcementLibMaterialToDTOConvertStrategy();
        public IUpdateStrategy<IHelperMaterial> SafetyFactorUpdateStrategy => safetyFactorUpdateStrategy ??= new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorToDTOLogic());
        public LibMaterialToDTOConvertStrategy<SteelLibMaterialDTO, ISteelLibMaterial> SteelConvertStrategy => steelConvertStrategy ??= new SteelLibMaterialToDTOConvertStrategy();
    }
}
