using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionRepositoryCloneStrategy : ICloneStrategy<ICrossSectionRepository>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly ICollectionCloneStrategy<IForceAction> forcesCloneStrategy;
        private readonly ICollectionCloneStrategy<IHeadMaterial> materialsCloneStrategy;
        private readonly ICollectionCloneStrategy<INdmPrimitive> primitivesCloneStrategy;
        private readonly ICollectionCloneStrategy<ICalculator> calculatorsCloneStrategy;
        private HasPrimitivesUpdateCloningStrategy hasPrimitivesUpdateCloningStrategy;
        private HasCalculatorsUpdateCloningStrategy hasCalculatorsUpdateCloningStrategy;

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
            hasPrimitivesUpdateCloningStrategy = new(cloningStrategy);
            hasCalculatorsUpdateCloningStrategy = new(cloningStrategy);
        }

        public ICrossSectionRepository GetClone(ICrossSectionRepository sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            var targetRepository = new CrossSectionRepository(id);
            Populate(targetRepository, sourceObject, cloneMode, cloneDepth);
            return targetRepository;
        }
         


        /// <summary>
        /// Replaces the contents of the target repository with cloned contents of the source.
        /// The target repository ID is preserved; cloned children receive the requested IDs.
        /// </summary>
        public void Populate(
            ICrossSectionRepository targetRepository,
            ICrossSectionRepository sourceRepository,
            CloneMode cloningMode,
            CloneDepth cloneDepth)
        {
            CheckObject.ThrowIfNull(targetRepository);
            CheckObject.ThrowIfNull(sourceRepository);
            
            targetRepository.Calculators.Clear();
            targetRepository.Primitives.Clear();
            targetRepository.HeadMaterials.Clear();
            targetRepository.ForceActions.Clear();
            forcesCloneStrategy.GetClone(targetRepository.ForceActions, sourceRepository.ForceActions, cloningMode);
            materialsCloneStrategy.GetClone(targetRepository.HeadMaterials, sourceRepository.HeadMaterials, cloningMode);
            hasPrimitivesUpdateCloningStrategy.Update(targetRepository, sourceRepository);
            targetRepository.Primitives.ForEach(x => x.CrossSectionRepository = targetRepository);
            hasCalculatorsUpdateCloningStrategy.Update(targetRepository, sourceRepository);
        }
    }
}
