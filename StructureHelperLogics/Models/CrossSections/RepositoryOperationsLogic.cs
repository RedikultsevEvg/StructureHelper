using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.CrossSections
{
    public class RepositoryOperationsLogic : IRepositoryOperationsLogic
    {
        private ICrossSectionRepository repository;
        private RepositoryPrimitiveOperation primitiveLogic;

        public RepositoryOperationsLogic(ICrossSectionRepository repository)
        {
            this.repository = repository;
        }

        public IRepositoryOperation<ICrossSectionRepository, INdmPrimitive> Primitives => primitiveLogic ??= new RepositoryPrimitiveOperation(repository);
    }
}
