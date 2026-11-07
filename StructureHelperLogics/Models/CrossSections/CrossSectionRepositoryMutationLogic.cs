using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelper.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperLogics.Models.CrossSections
{
    /// <summary>
    /// Shared mutation rules for the cross-section repository and its DTO representation.
    /// </summary>
    public static class CrossSectionRepositoryMutationLogic
    {
        public static void AddHeadMaterial(ICrossSectionRepository repository, IHeadMaterial item)
        {
            Add(repository.HeadMaterials, item, "Head material");
        }

        public static bool RemoveHeadMaterial(ICrossSectionRepository repository, IHeadMaterial item)
        {
            var existing = Find(repository.HeadMaterials, item);
            return existing is not null && RemoveHeadMaterialCore(repository, existing);
        }

        public static bool RemoveHeadMaterial(ICrossSectionRepository repository, Guid id)
        {
            var existing = repository.HeadMaterials.FirstOrDefault(x => x.Id == id);
            return existing is not null && RemoveHeadMaterialCore(repository, existing);
        }

        public static void AddForceAction(ICrossSectionRepository repository, IForceAction item)
        {
            Add(repository.ForceActions, item, "Force action");
        }

        public static bool RemoveForceAction(ICrossSectionRepository repository, IForceAction item)
        {
            var existing = Find(repository.ForceActions, item);
            if (existing is null)
            {
                return false;
            }
            repository.Operations.Actions.Remove(existing);
            return true;
        }

        public static bool RemoveForceAction(ICrossSectionRepository repository, Guid id)
        {
            var existing = repository.ForceActions.FirstOrDefault(x => x.Id == id);
            return existing is not null && RemoveForceAction(repository, existing);
        }

        public static void AddPrimitive(ICrossSectionRepository repository, INdmPrimitive item)
        {
            Add(repository.Primitives, item, "Primitive");
        }

        public static bool RemovePrimitive(ICrossSectionRepository repository, INdmPrimitive item)
        {
            var existing = Find(repository.Primitives, item);
            if (existing is null)
            {
                return false;
            }
            repository.Operations.Primitives.Remove(existing);
            return true;
        }

        public static bool RemovePrimitive(ICrossSectionRepository repository, Guid id)
        {
            var existing = repository.Primitives.FirstOrDefault(x => x.Id == id);
            return existing is not null && RemovePrimitive(repository, existing);
        }

        public static void AddCalculator(ICrossSectionRepository repository, ICalculator item)
        {
            Add(repository.Calculators, item, "Calculator");
        }

        public static bool RemoveCalculator(ICrossSectionRepository repository, ICalculator item)
        {
            var existing = Find(repository.Calculators, item);
            if (existing is null)
            {
                return false;
            }
            repository.Calculators.Remove(existing);
            return true;
        }

        public static bool RemoveCalculator(ICrossSectionRepository repository, Guid id)
        {
            var existing = repository.Calculators.FirstOrDefault(x => x.Id == id);
            return existing is not null && RemoveCalculator(repository, existing);
        }

        public static void ClearAll(ICrossSectionRepository repository)
        {
            repository.Calculators.Clear();
            repository.Operations.Primitives.RemoveAll();
            repository.Operations.Actions.RemoveAll();
            repository.HeadMaterials.Clear();
        }

        private static void Add<T>(List<T> items, T item, string name)
            where T : ISaveable
        {
            CheckObject.ThrowIfNull(item, name);
            if (items.Any(x => ReferenceEquals(x, item) || x.Id == item.Id))
            {
                throw new StructureHelperException(
                    $"{ErrorStrings.DataIsInCorrect}: {name} with Id = {item.Id} already exists");
            }
            items.Add(item);
        }

        private static T? Find<T>(List<T> items, T? item)
            where T : ISaveable
        {
            if (item is null)
            {
                return default;
            }
            return items.FirstOrDefault(x => ReferenceEquals(x, item) || x.Id == item.Id);
        }

        private static bool RemoveHeadMaterialCore(ICrossSectionRepository repository, IHeadMaterial existing)
        {
            var usingPrimitive = repository.Primitives
                .FirstOrDefault(x => x.NdmElement?.HeadMaterial is IHeadMaterial material
                    && (ReferenceEquals(material, existing) || material.Id == existing.Id));
            if (usingPrimitive is not null)
            {
                throw new StructureHelperException(
                    $"{ErrorStrings.DataIsInCorrect}: head material Id = {existing.Id} is used by primitive Id = {usingPrimitive.Id}");
            }
            return repository.HeadMaterials.Remove(existing);
        }
    }
}
