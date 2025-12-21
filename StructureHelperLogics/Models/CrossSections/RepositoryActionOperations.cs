using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperLogics.Models.CrossSections
{
    public class RepositoryActionOperations : IRepositoryOperation<ICrossSectionRepository, IForceAction>
    {
        private ICrossSectionRepository repository;

        public RepositoryActionOperations(ICrossSectionRepository repository)
        {
            this.repository = repository;
        }

        public void Remove(IForceAction entity)
        {
            foreach (var calc in repository.Calculators)
            {
                if (calc is IForceCalculator forceCalculator)
                {
                    forceCalculator.InputData.ForceActions.Remove(entity);
                }
                else if (calc is ICrackCalculator crackCalculator)
                {
                    crackCalculator.InputData.ForceActions.Remove(entity);
                }
                else if (calc is ILimitCurvesCalculator)
                {
                    //nothing to do
                }
                else if (calc is IValueDiagramCalculator diagramCalculator)
                {
                    diagramCalculator.InputData.ForceActions.Remove(entity);
                }
                else if (calc is ICurvatureCalculator curvatureCalculator)
                {
                    curvatureCalculator.InputData.ForceActions.Remove(entity);
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(ICalculator), calc));
                }
            }
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
            throw new NotImplementedException();
        }
    }
}
