using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Models.Materials.Logics
{
    /// <summary>
    /// Updates a complete custom concrete helper material, including its models and safety factors.
    /// Models with matching IDs are updated in place; missing models are created from the source.
    /// </summary>
    public class CustomConcreteHelperMaterialUpdateStrategy : IParentUpdateStrategy<ICustomConcreteHelperMaterial>
    {
        private readonly IUpdateStrategy<ICustomConcreteModel> modelUpdateStrategy;
        private readonly IUpdateStrategy<IHelperMaterial> safetyFactorsUpdateStrategy;

        public CustomConcreteHelperMaterialUpdateStrategy()
            : this(
                new CustomConcreteModelUpdateStrategy(),
                new HelpermaterialSafetyFactorsUpdateStrategy())
        {
        }

        public CustomConcreteHelperMaterialUpdateStrategy(
            IUpdateStrategy<ICustomConcreteModel> modelUpdateStrategy,
            IUpdateStrategy<IHelperMaterial> safetyFactorsUpdateStrategy)
        {
            this.modelUpdateStrategy = modelUpdateStrategy;
            this.safetyFactorsUpdateStrategy = safetyFactorsUpdateStrategy;
        }

        public bool UpdateChildren { get; set; } = true;

        public void Update(
            ICustomConcreteHelperMaterial targetObject,
            ICustomConcreteHelperMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return;  }

            CheckObject.ThrowIfNull(
                sourceObject.MaterialModels,
                $"{nameof(sourceObject)}.{nameof(sourceObject.MaterialModels)}");
            CheckObject.ThrowIfNull(
                targetObject.MaterialModels,
                $"{nameof(targetObject)}.{nameof(targetObject.MaterialModels)}");
            targetObject.TensionForULS = sourceObject.TensionForULS;
            targetObject.TensionForSLS = sourceObject.TensionForSLS;

            if (UpdateChildren == true)
            {
                safetyFactorsUpdateStrategy.Update(targetObject, sourceObject);
                List<ICustomConcreteModel> targetModels = targetObject.MaterialModels
                    .Where(x => x is not null)
                    .ToList();
                var targetById = targetModels
                    .GroupBy(x => x.Id)
                    .ToDictionary(group => group.Key, group => group.First());

                var updatedModels = new List<ICustomConcreteModel>();
                foreach (ICustomConcreteModel sourceModel in sourceObject.MaterialModels)
                {
                    CheckObject.ThrowIfNull(sourceModel, nameof(sourceModel));
                    if (targetById.TryGetValue(sourceModel.Id, out ICustomConcreteModel targetModel))
                    {
                        targetById.Remove(sourceModel.Id);
                        modelUpdateStrategy.Update(targetModel, sourceModel);
                        updatedModels.Add(targetModel);
                    }
                    else
                    {
                        ICustomConcreteModel newModel = CreateModel(sourceModel);
                        modelUpdateStrategy.Update(newModel, sourceModel);
                        updatedModels.Add(newModel);
                    }
                }
                targetObject.MaterialModels.Clear();
                targetObject.MaterialModels.AddRange(updatedModels);
            }





        }

        private static ICustomConcreteModel CreateModel(ICustomConcreteModel sourceObject)
        {
            if (sourceObject is CustomConcreteModel)
            {
                return new CustomConcreteModel(sourceObject.Id);
            }
            if (sourceObject is ICloneable cloneable)
            {
                if (cloneable.Clone() is ICustomConcreteModel clone)
                {
                    return clone;
                }
            }
            throw new StructureHelperException(
                $"{nameof(ICustomConcreteModel)} implementation does not support cloning.");
        }
    }
}
