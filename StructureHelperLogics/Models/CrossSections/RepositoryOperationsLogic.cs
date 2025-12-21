using NLog.LayoutRenderers;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.CrossSections
{
    public class RepositoryOperationsLogic : IRepositoryOperationsLogic
    {
        private ICrossSectionRepository repository;
        private RepositoryPrimitiveOperation primitiveLogic;
        private IRepositoryOperation<ICrossSectionRepository, IForceAction> actionLogic;

        public RepositoryOperationsLogic(ICrossSectionRepository repository)
        {
            this.repository = repository;
        }

        public IRepositoryOperation<ICrossSectionRepository, INdmPrimitive> Primitives => primitiveLogic ??= new RepositoryPrimitiveOperation(repository);

        public IRepositoryOperation<ICrossSectionRepository, IForceAction> Actions => actionLogic ??=  new RepositoryActionOperations(repository);
    }
}
