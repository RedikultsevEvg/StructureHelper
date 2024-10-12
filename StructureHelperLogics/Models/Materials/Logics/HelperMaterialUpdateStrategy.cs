using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class HelperMaterialUpdateStrategy : IUpdateStrategy<IHelperMaterial>
    {
        private IUpdateStrategy<IElasticMaterial> elasticStrategy;
        private IUpdateStrategy<IFRMaterial> frStrategy;
        private IUpdateStrategy<IConcreteLibMaterial> concreteStrategy;
        private IUpdateStrategy<IReinforcementLibMaterial> reinforcementStrategy;
        private IUpdateStrategy<IHelperMaterial> safetyFactorUpdateStrategy = new HelpermaterialSafetyFactorsUpdateStrategy();
        public HelperMaterialUpdateStrategy(IUpdateStrategy<IElasticMaterial> elasticStrategy,
            IUpdateStrategy<IFRMaterial> frStrategy,
            IUpdateStrategy<IConcreteLibMaterial> concreteStrategy,
            IUpdateStrategy<IReinforcementLibMaterial> reinforcementStrategy
            )
        {
            this.elasticStrategy = elasticStrategy;
            this.frStrategy = frStrategy;
            this.concreteStrategy = concreteStrategy;
            this.reinforcementStrategy = reinforcementStrategy;
        }
        public HelperMaterialUpdateStrategy() : this(
            new ElasticUpdateStrategy(),
            new FRUpdateStrategy(),
            new ConcreteLibUpdateStrategy(),
            new ReinforcementLibUpdateStrategy()
        )
        { }

        public void Update(IHelperMaterial targetObject, IHelperMaterial sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            safetyFactorUpdateStrategy.Update(targetObject, sourceObject);
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
