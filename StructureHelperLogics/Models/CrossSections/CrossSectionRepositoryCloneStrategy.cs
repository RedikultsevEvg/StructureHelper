using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelper.Models.Materials;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionRepositoryCloneStrategy : ICloneStrategy<ICrossSectionRepository>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly ICollectionCloneStrategy<IForceAction> forcesCloneStrategy;
        private readonly ICollectionCloneStrategy<IHeadMaterial> materialsCloneStrategy;
        private readonly ICollectionCloneStrategy<INdmPrimitive> primitivesCloneStrategy;
        private readonly ICollectionCloneStrategy<ICalculator> calculatorsCloneStrategy;

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
        }

        public CrossSectionRepositoryCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            forcesCloneStrategy = new HasForceActionsCloneStrategy(cloningStrategy);
            materialsCloneStrategy = new HasHeadMaterialsCloneStrategy(cloningStrategy);
            primitivesCloneStrategy = new HasPrimitivesCloneStrategy(cloningStrategy);
            calculatorsCloneStrategy = new HasCalculatorsCloneStrategy(cloningStrategy);
        }

        public ICrossSectionRepository GetClone(ICrossSectionRepository sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            var targetRepository = new CrossSectionRepository(id);
            Populate(targetRepository, sourceObject, cloningMode);
            return targetRepository;
        }

        /// <summary>
        /// Replaces the contents of the target repository with cloned contents of the source.
        /// The target repository ID is preserved; cloned children receive the requested IDs.
        /// </summary>
        public void Populate(
            ICrossSectionRepository targetRepository,
            ICrossSectionRepository sourceObject,
            CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(targetRepository);
            CheckObject.ThrowIfNull(sourceObject);
            forcesCloneStrategy.GetClone(targetRepository.ForceActions, sourceObject.ForceActions, cloningMode);
            materialsCloneStrategy.GetClone(targetRepository.HeadMaterials, sourceObject.HeadMaterials, cloningMode);
            primitivesCloneStrategy.GetClone(targetRepository.Primitives, sourceObject.Primitives, cloningMode);
            calculatorsCloneStrategy.GetClone(targetRepository.Calculators, sourceObject.Calculators, cloningMode);
        }
    }
}
