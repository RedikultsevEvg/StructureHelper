using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.Materials
{
    public class HeadMaterialUpdateStrategy : IUpdateStrategy<IHeadMaterial>
    {
        private IUpdateStrategy<IElasticMaterial> elasticStrategy;
        private IUpdateStrategy<IFRMaterial> frStrategy;
        private IUpdateStrategy<IConcreteLibMaterial> concreteStrategy;
        private IUpdateStrategy<IReinforcementLibMaterial> reinforcementStrategy;
        public HeadMaterialUpdateStrategy(IUpdateStrategy<IElasticMaterial> elasticStrategy,
            IUpdateStrategy<IFRMaterial> frStrategy,
            IUpdateStrategy<IConcreteLibMaterial> concreteStrategy,
            IUpdateStrategy<IReinforcementLibMaterial> reinforcementStrategy
            )
        {
            this.elasticStrategy = elasticStrategy;
            this.frStrategy = frStrategy;
            this.concreteStrategy = concreteStrategy;
            this.reinforcementStrategy= reinforcementStrategy;
        }
        public HeadMaterialUpdateStrategy() : this(
            new ElasticUpdateStrategy(),
            new FRUpdateStrategy(),
            new ConcreteLibUpdateStrategy(),
            new ReinforcementLibUpdateStrategy()
        )  {         }

        public void Update(IHeadMaterial targetObject, IHeadMaterial sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.Color = sourceObject.Color;
            targetObject.HelperMaterial = sourceObject.HelperMaterial.Clone() as IHelperMaterial;
            UpdateHelperMaterial(targetObject.HelperMaterial, sourceObject.HelperMaterial);
        }

        private void UpdateHelperMaterial(IHelperMaterial targetObject, IHelperMaterial sourceObject)
        {
            CheckObject.CompareTypes(targetObject, sourceObject);
            if (sourceObject is ILibMaterial)
            {
                UpdateLibMaterial(targetObject, sourceObject);
            }
            else if (sourceObject is IElasticMaterial)
            {
                elasticStrategy.Update(targetObject as IElasticMaterial, sourceObject as IElasticMaterial);
            }
            else if (sourceObject is IFRMaterial)
            {
                frStrategy.Update(targetObject as IFRMaterial, sourceObject as IFRMaterial);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(IHelperMaterial), sourceObject.GetType());
            }
        }

        private void UpdateLibMaterial(IHelperMaterial targetObject, IHelperMaterial sourceObject)
        {
            if (sourceObject is IConcreteLibMaterial)
            {
                concreteStrategy.Update(targetObject as IConcreteLibMaterial, sourceObject as IConcreteLibMaterial);
            }
            else if (sourceObject is IReinforcementLibMaterial)
            {
                reinforcementStrategy.Update(targetObject as IReinforcementLibMaterial, sourceObject as IReinforcementLibMaterial);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(ILibMaterial), sourceObject.GetType());
            }
        }
    }
}
