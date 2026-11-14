using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionRepository : ICrossSectionRepository
    {
        private readonly List<IForceAction> forceActions = [];
        private readonly List<IHeadMaterial> headMaterials = [];
        private readonly List<INdmPrimitive> primitives = [];
        private readonly List<ICalculator> calculators = [];
        private RepositoryOperationsLogic? operations;

        public Guid Id { get; }
        public List<IForceAction> ForceActions => forceActions;
        public List<IHeadMaterial> HeadMaterials => headMaterials;
        public List<INdmPrimitive> Primitives => primitives;
        public List<ICalculator> Calculators => calculators;

        public IRepositoryOperationsLogic Operations => operations ??= new RepositoryOperationsLogic(this);

        public CrossSectionRepository(Guid id)
        {
            Id = id;
        }

        public CrossSectionRepository() : this(Guid.NewGuid())
        {
        }

        public void AddHeadMaterial(IHeadMaterial item)
        {
            CrossSectionRepositoryMutationLogic.AddHeadMaterial(this, item);
        }

        public bool RemoveHeadMaterial(IHeadMaterial item)
        {
            return CrossSectionRepositoryMutationLogic.RemoveHeadMaterial(this, item);
        }

        public bool RemoveHeadMaterial(Guid id)
        {
            return CrossSectionRepositoryMutationLogic.RemoveHeadMaterial(this, id);
        }

        public void AddForceAction(IForceAction item)
        {
            CrossSectionRepositoryMutationLogic.AddForceAction(this, item);
        }

        public bool RemoveForceAction(IForceAction item)
        {
            return CrossSectionRepositoryMutationLogic.RemoveForceAction(this, item);
        }

        public bool RemoveForceAction(Guid id)
        {
            return CrossSectionRepositoryMutationLogic.RemoveForceAction(this, id);
        }

        public void AddPrimitive(INdmPrimitive item)
        {
            CrossSectionRepositoryMutationLogic.AddPrimitive(this, item);
        }

        public bool RemovePrimitive(INdmPrimitive item)
        {
            return CrossSectionRepositoryMutationLogic.RemovePrimitive(this, item);
        }

        public bool RemovePrimitive(Guid id)
        {
            return CrossSectionRepositoryMutationLogic.RemovePrimitive(this, id);
        }

        public void AddCalculator(ICalculator item)
        {
            CrossSectionRepositoryMutationLogic.AddCalculator(this, item);
        }

        public bool RemoveCalculator(ICalculator item)
        {
            return CrossSectionRepositoryMutationLogic.RemoveCalculator(this, item);
        }

        public bool RemoveCalculator(Guid id)
        {
            return CrossSectionRepositoryMutationLogic.RemoveCalculator(this, id);
        }

        public void ClearAll()
        {
            CrossSectionRepositoryMutationLogic.ClearAll(this);
        }

        public ICrossSectionRepository GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            var deepCloningStrategy = new DeepCloningStrategy();
            var cloneStrategy = new CrossSectionRepositoryCloneStrategy(deepCloningStrategy);
            return cloneStrategy.GetClone(this, cloneMode, cloneDepth);
        }
    }
}
