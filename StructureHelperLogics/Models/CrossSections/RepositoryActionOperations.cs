using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.Models.CrossSections
{
    public class RepositoryActionOperations : IRepositoryOperation<ICrossSectionRepository, IForceAction>
    {
        private readonly ICrossSectionRepository repository;

        public RepositoryActionOperations(ICrossSectionRepository repository)
        {
            this.repository = repository;
        }

        public void Remove(IForceAction entity)
        {
            if (entity is null)
            {
                return;
            }

            CrossSectionCalculatorCleanupLogic.RemoveAction(repository, entity);
            repository.ForceActions.Remove(entity);
        }

        public void Remove(IEnumerable<IForceAction> entities)
        {
            foreach (var item in entities)
            {
                Remove(item);
            }
        }

        public void RemoveAll()
        {
            CrossSectionCalculatorCleanupLogic.ClearActions(repository);
            repository.ForceActions.Clear();
        }
    }
}
