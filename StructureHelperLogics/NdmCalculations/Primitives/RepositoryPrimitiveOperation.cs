using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class RepositoryPrimitiveOperation : IRepositoryOperation<ICrossSectionRepository, INdmPrimitive>
    {
        private readonly ICrossSectionRepository repository;

        public RepositoryPrimitiveOperation(ICrossSectionRepository repository)
        {
            this.repository = repository;
        }

        public void RemoveAll()
        {
            CrossSectionCalculatorCleanupLogic.ClearPrimitives(repository);
            repository.Primitives.Clear();
        }

        public void Remove(IEnumerable<INdmPrimitive> entities)
        {
            foreach (var item in entities)
            {
                Remove(item);
            }
        }

        public void Remove(INdmPrimitive entity)
        {
            if (entity is null)
            {
                return;
            }

            var childPrimitives = repository.Primitives
                .Where(x => x is IHasHostPrimitive)
                .Where(x => (x as IHasHostPrimitive)?.HostPrimitive == entity);
            foreach (var child in childPrimitives)
            {
                (child as IHasHostPrimitive)!.HostPrimitive = null;
            }

            CrossSectionCalculatorCleanupLogic.RemovePrimitive(repository, entity);
            repository.Primitives.Remove(entity);
        }
    }
}
