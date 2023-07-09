using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class MaterialUpdateStrategy : IUpdateStrategy<IHeadMaterial>
    {
        private readonly IUpdateStrategy<IElasticMaterial> elasticStrategy = new ElasticUpdateStrategy();
        private readonly IUpdateStrategy<IFRMaterial> frStrategy = new FRUpdateStrategy();
        private readonly IUpdateStrategy<IConcreteLibMaterial> concreteStrategy = new ConcreteLibUpdateStrategy();
        private readonly IUpdateStrategy<IReinforcementLibMaterial> reinforcementStrategy = new ReinforcementLibUpdateStrategy();
        public void Update(IHeadMaterial targetObject, IHeadMaterial sourceObject)
        {
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
