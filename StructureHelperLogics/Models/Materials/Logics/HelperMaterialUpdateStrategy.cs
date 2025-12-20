using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials.Logics;

namespace StructureHelperLogics.Models.Materials
{
    public class HelperMaterialUpdateStrategy : IUpdateStrategy<IHelperMaterial>
    {
        private IUpdateStrategy<IElasticMaterial> elasticUpdateStrategy;
        private IUpdateStrategy<IFRMaterial> frUpdateStrategy;
        private IUpdateStrategy<IConcreteLibMaterial> concreteUpdateStrategy;
        private IUpdateStrategy<IReinforcementLibMaterial> reinforcementUpdateStrategy;
        private IUpdateStrategy<ISteelLibMaterial> steelUpdateStrategy;
        private IUpdateStrategy<IElasticMaterial> ElasticUpdateStrategy => elasticUpdateStrategy ??= new ElasticUpdateStrategy();
        private IUpdateStrategy<IFRMaterial> FrUpdateStrategy => frUpdateStrategy ??= new FRUpdateStrategy();
        private IUpdateStrategy<IConcreteLibMaterial> ConcreteUpdateStrategy => concreteUpdateStrategy ??= new ConcreteLibUpdateStrategy();
        private IUpdateStrategy<IReinforcementLibMaterial> ReinforcementUpdateStrategy => reinforcementUpdateStrategy ??= new ReinforcementLibUpdateStrategy();
        private IUpdateStrategy<ISteelLibMaterial> SteelUpdateStrategy => steelUpdateStrategy ??= new SteelLibMaterialUpdateStrategy();
        private IUpdateStrategy<IHelperMaterial> safetyFactorUpdateStrategy = new HelpermaterialSafetyFactorsUpdateStrategy();
        public HelperMaterialUpdateStrategy(IUpdateStrategy<IElasticMaterial> elasticStrategy,
            IUpdateStrategy<IFRMaterial> frStrategy,
            IUpdateStrategy<IConcreteLibMaterial> concreteStrategy,
            IUpdateStrategy<IReinforcementLibMaterial> reinforcementStrategy
            )
        {
            this.elasticUpdateStrategy = elasticStrategy;
            this.frUpdateStrategy = frStrategy;
            this.concreteUpdateStrategy = concreteStrategy;
            this.reinforcementUpdateStrategy = reinforcementStrategy;
        }
        public HelperMaterialUpdateStrategy()
        { }

        public void Update(IHelperMaterial targetObject, IHelperMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            safetyFactorUpdateStrategy.Update(targetObject, sourceObject);
            if (sourceObject is ILibMaterial)
            {
                UpdateLibMaterial(targetObject, sourceObject);
            }
            else if (sourceObject is IElasticMaterial)
            {
                ElasticUpdateStrategy.Update(targetObject as IElasticMaterial, sourceObject as IElasticMaterial);
            }
            else if (sourceObject is IFRMaterial)
            {
                FrUpdateStrategy.Update(targetObject as IFRMaterial, sourceObject as IFRMaterial);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(IHelperMaterial), sourceObject.GetType());
            }
        }

        private void UpdateLibMaterial(IHelperMaterial targetObject, IHelperMaterial sourceObject)
        {
            if (sourceObject is IConcreteLibMaterial concreteLibMaterial)
            {
                ConcreteUpdateStrategy.Update(targetObject as IConcreteLibMaterial, concreteLibMaterial);
            }
            else if (sourceObject is IReinforcementLibMaterial reinforcementLibMaterial)
            {
                ReinforcementUpdateStrategy.Update(targetObject as IReinforcementLibMaterial, reinforcementLibMaterial);
            }
            else if (sourceObject is ISteelLibMaterial steelLibMaterial)
            {
                SteelUpdateStrategy.Update(targetObject as ISteelLibMaterial, steelLibMaterial);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(ILibMaterial), sourceObject.GetType());
            }
        }
    }
}
