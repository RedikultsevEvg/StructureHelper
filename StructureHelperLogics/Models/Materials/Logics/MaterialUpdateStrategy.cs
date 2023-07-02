using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class MaterialUpdateStrategy : IUpdateStrategy<IHeadMaterial>
    {
        public void Update(IHeadMaterial targetObject, IHeadMaterial sourceObject)
        {
            targetObject.Name = sourceObject.Name;
            targetObject.Color = sourceObject.Color;
            targetObject.HelperMaterial = sourceObject.HelperMaterial.Clone() as IHelperMaterial;
            UpdateHelperMaterial(targetObject.HelperMaterial, sourceObject.HelperMaterial);
        }

        private static void UpdateHelperMaterial(IHelperMaterial target, IHelperMaterial source)
        {
            Check(target, source);
            UpdateMaterial(target, source);
        }
        private static void Check(IHelperMaterial target, IHelperMaterial source)
        {
            if (target.GetType() != source.GetType())
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $"target type is {target.GetType()}, \n is no coinside with source type {source.GetType()}");
            }
        }
        private static void UpdateMaterial(IHelperMaterial target, IHelperMaterial source)
        {
            if (source is ILibMaterial)
            {
                UpdateLibMaterial(target, source);
            }
            else if (source is IElasticMaterial)
            {
                UpdateElastic(target, source);
            }
            else if (source is IFRMaterial)
            {
                UpdateFR(target, source);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $"\n Expected: {typeof(IHelperMaterial)},\n But was: {source.GetType()}");
            }
        }
        private static void UpdateFR(IHelperMaterial target, IHelperMaterial source)
        {
            var targetMaterial = target as IFRMaterial;
            var sourceMaterial = source as IFRMaterial;
            var logic = new FRUpdateStrategy();
            logic.Update(targetMaterial, sourceMaterial);
        }
        private static void UpdateElastic(IHelperMaterial target, IHelperMaterial source)
        {
            var targetMaterial = target as IElasticMaterial;
            var sourceMaterial = source as IElasticMaterial;
            var logic = new ElasticUpdateStrategy();
            logic.Update(targetMaterial, sourceMaterial);
        }
        private static void UpdateLibMaterial(IHelperMaterial target, IHelperMaterial source)
        {
            if (source is IConcreteLibMaterial)
            {
                var targetMaterial = target as IConcreteLibMaterial;
                var sourceMaterial = source as IConcreteLibMaterial;
                var logic = new ConcreteLibUpdateStrategy();
                logic.Update(targetMaterial, sourceMaterial);
            }
            else if (source is IReinforcementLibMaterial)
            {
                var targetMaterial = target as IReinforcementLibMaterial;
                var sourceMaterial = source as IReinforcementLibMaterial;
                var logic = new ReinforcementLibUpdateStrategy();
                logic.Update(targetMaterial, sourceMaterial);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $"\n Expected: {typeof(ILibMaterial)},\n But was: {source.GetType()}");
            }
        }
    }
}
