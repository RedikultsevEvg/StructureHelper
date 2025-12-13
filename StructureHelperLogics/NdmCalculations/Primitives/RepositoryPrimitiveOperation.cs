using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class RepositoryPrimitiveOperation : IRepositoryOperation<ICrossSectionRepository, INdmPrimitive>
    {
        private ICrossSectionRepository repository;

        public RepositoryPrimitiveOperation(ICrossSectionRepository repository)
        {
            this.repository = repository;
        }

        public void RemoveAll()
        {
            foreach (var calculator in repository.Calculators)
            {
                if (calculator is IForceCalculator forceCalculator)
                {
                    forceCalculator.InputData.Primitives.Clear();
                }
                else if (calculator is ICrackCalculator crackCalculator)
                {
                    crackCalculator.InputData.Primitives.Clear();
                }
                else if (calculator is IValueDiagramCalculator valueDiagramCalculator)
                {
                    valueDiagramCalculator.InputData.Primitives.Clear();
                }
                else if (calculator is ICurvatureCalculator curvatureCalculator)
                {
                    curvatureCalculator.InputData.Primitives.Clear();
                }
                else if (calculator is ILimitCurvesCalculator limitCurve)
                {
                    // skip
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(calculator) + $": Unsupported calculator type: { calculator.GetType().FullName }");
                }
            }
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
            foreach (var calculator in repository.Calculators)
            {
                if (calculator is IForceCalculator forceCalculator)
                {
                    forceCalculator.InputData.Primitives.Remove(entity);
                }
                else if (calculator is ICrackCalculator crackCalculator)
                {
                    crackCalculator.InputData.Primitives.Remove(entity);
                }
                else if (calculator is IValueDiagramCalculator valueDiagramCalculator)
                {
                    valueDiagramCalculator.InputData.Primitives.Remove(entity);
                }
                else if (calculator is ICurvatureCalculator curvatureCalculator)
                {
                    curvatureCalculator.InputData.Primitives.Remove(entity);
                }
                else if (calculator is ILimitCurvesCalculator limitCurve)
                {
                    // skip, nothing to do
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(calculator) + $": Unsupported calculator type: {calculator.GetType().FullName}");
                }
            }
            repository.Primitives.Remove(entity);
        }
    }
}
