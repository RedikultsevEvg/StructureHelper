using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelper.Models.Materials;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionRepositoryCloneStrategy : ICloneStrategy<ICrossSectionRepository>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly ICollectionCloneStrategy<IForceAction> forcesCloneStrategy;
        private readonly ICollectionCloneStrategy<IHeadMaterial> materialsCloneStrategy;
        private readonly ICollectionCloneStrategy<INdmPrimitive> primitivesCloneStrategy;
        private readonly ICollectionCloneStrategy<ICalculator> calculatorsCloneStrategy;
        private readonly ICloneStrategy<IForceAction> forceActionCloneStrategy;
        private readonly ICloneStrategy<IHeadMaterial> headMaterialCloneStrategy;
        private readonly ICloneStrategy<INdmPrimitive> ndmPrimitiveCloneStrategy;
        private readonly ICloneStrategy<ICalculator> calculatorCloneStrategy;

        public CrossSectionRepositoryCloneStrategy(
            ICloningStrategy cloningStrategy,
            ICollectionCloneStrategy<IForceAction> forcesCloneStrategy,
            ICollectionCloneStrategy<IHeadMaterial> materialsCloneStrategy,
            ICollectionCloneStrategy<INdmPrimitive> primitivesCloneStrategy,
            ICollectionCloneStrategy<ICalculator> calculatorsCloneStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forcesCloneStrategy = forcesCloneStrategy;
            this.materialsCloneStrategy = materialsCloneStrategy;
            this.primitivesCloneStrategy = primitivesCloneStrategy;
            this.calculatorsCloneStrategy = calculatorsCloneStrategy;
            forceActionCloneStrategy = new ForceActionCloneStrategy();
            headMaterialCloneStrategy = new HeadMaterialCloneStrategy();
            ndmPrimitiveCloneStrategy = new NdmPrimitiveCloneStrategy(cloningStrategy);
            calculatorCloneStrategy = new CalculatorCloneStrategy();
        }

        public CrossSectionRepositoryCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            forcesCloneStrategy = new HasForceActionsCloneStrategy(cloningStrategy);
            materialsCloneStrategy = new HasHeadMaterialsCloneStrategy(cloningStrategy);
            primitivesCloneStrategy = new HasPrimitivesCloneStrategy(cloningStrategy);
            calculatorsCloneStrategy = new HasCalculatorsCloneStrategy(cloningStrategy);
            forceActionCloneStrategy = new ForceActionCloneStrategy();
            headMaterialCloneStrategy = new HeadMaterialCloneStrategy();
            ndmPrimitiveCloneStrategy = new NdmPrimitiveCloneStrategy(cloningStrategy);
            calculatorCloneStrategy = new CalculatorCloneStrategy();
        }

        public ICrossSectionRepository GetClone(ICrossSectionRepository sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            var targetRepository = new CrossSectionRepository(id);
            if (cloneDepth == CloneDepth.PreserveReference)
            {
                PopulatePreservingReferences(targetRepository, sourceObject, cloneMode);
            }
            else
            {
                Populate(targetRepository, sourceObject, cloneMode, cloneDepth);
            }
            return targetRepository;
        }

        /// <summary>
        /// Clones the repository while preserving reference identity inside the clone.
        /// Every top-level object is cloned once; references from primitives, force actions
        /// and calculators to those objects are then rewired to the same cloned instances.
        /// </summary>
        private void PopulatePreservingReferences(
            ICrossSectionRepository targetRepository,
            ICrossSectionRepository sourceObject,
            CloneMode cloneMode)
        {
            var sourceToClone = new Dictionary<object, object>(ReferenceEqualityComparer.Instance);

            var materials = CloneItems(
                sourceObject.HeadMaterials,
                headMaterialCloneStrategy,
                cloneMode,
                sourceToClone);
            var primitives = CloneItems(
                sourceObject.Primitives,
                ndmPrimitiveCloneStrategy,
                cloneMode,
                sourceToClone);
            var actions = CloneItems(
                sourceObject.ForceActions,
                forceActionCloneStrategy,
                cloneMode,
                sourceToClone);
            var calculators = CloneItems(
                sourceObject.Calculators,
                calculatorCloneStrategy,
                cloneMode,
                sourceToClone);

            targetRepository.HeadMaterials.Clear();
            targetRepository.HeadMaterials.AddRange(materials);

            targetRepository.Primitives.Clear();
            targetRepository.Primitives.AddRange(primitives);

            targetRepository.ForceActions.Clear();
            targetRepository.ForceActions.AddRange(actions);

            targetRepository.Calculators.Clear();
            targetRepository.Calculators.AddRange(calculators);

            ReferenceIntegrityRepair.Repair(targetRepository, sourceToClone);
        }

        private List<T> CloneItems<T>(
            IEnumerable<T> sourceItems,
            ICloneStrategy<T> itemCloneStrategy,
            CloneMode cloneMode,
            IDictionary<object, object> sourceToClone)
            where T : class
        {
            var clones = new List<T>();
            foreach (var sourceItem in sourceItems)
            {
                var clone = cloningStrategy.Clone(sourceItem, itemCloneStrategy, cloneMode);
                sourceToClone[sourceItem] = clone;
                clones.Add(clone);
            }
            return clones;
        }

        /// <summary>
        /// Replaces the contents of the target repository with cloned contents of the source.
        /// The target repository ID is preserved; cloned children receive the requested IDs.
        /// </summary>
        public void Populate(
            ICrossSectionRepository targetRepository,
            ICrossSectionRepository sourceObject,
            CloneMode cloningMode,
            CloneDepth cloneDepth)
        {
            CheckObject.ThrowIfNull(targetRepository);
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneDepth == CloneDepth.PreserveReference)
            {
                PopulatePreservingReferences(targetRepository, sourceObject, cloningMode);
                return;
            }

            forcesCloneStrategy.GetClone(targetRepository.ForceActions, sourceObject.ForceActions, cloningMode);
            materialsCloneStrategy.GetClone(targetRepository.HeadMaterials, sourceObject.HeadMaterials, cloningMode);
            primitivesCloneStrategy.GetClone(targetRepository.Primitives, sourceObject.Primitives, cloningMode);
            calculatorsCloneStrategy.GetClone(targetRepository.Calculators, sourceObject.Calculators, cloningMode);
        }
    }
}
