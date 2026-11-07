using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.CrossSections
{
    /// <summary>
    /// Keeps calculator input data in sync when repository collections change.
    /// </summary>
    public static class CrossSectionCalculatorCleanupLogic
    {
        public static void RemovePrimitive(ICrossSectionRepository repository, INdmPrimitive primitive)
        {
            foreach (var calculator in repository.Calculators)
            {
                if (calculator is IForceCalculator forceCalculator)
                {
                    forceCalculator.InputData.Primitives.Remove(primitive);
                }
                else if (calculator is ICrackCalculator crackCalculator)
                {
                    crackCalculator.InputData.Primitives.Remove(primitive);
                }
                else if (calculator is IValueDiagramCalculator valueDiagramCalculator)
                {
                    valueDiagramCalculator.InputData.Primitives.Remove(primitive);
                }
                else if (calculator is ICurvatureCalculator curvatureCalculator)
                {
                    curvatureCalculator.InputData.Primitives.Remove(primitive);
                }
                else if (calculator is ILimitCurvesCalculator)
                {
                    // Limit-curve calculators do not keep repository primitive references.
                }
                else if (calculator is IHistoryCalculator historyCalculator)
                {
                    foreach (var stage in historyCalculator.InputData.ConstructionStages)
                    {
                        stage.InputData.Primitives.Remove(primitive);
                    }
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(ICalculator), calculator));
                }
            }
        }

        public static void ClearPrimitives(ICrossSectionRepository repository)
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
                else if (calculator is ILimitCurvesCalculator)
                {
                    // Limit-curve calculators do not keep repository primitive references.
                }
                else if (calculator is IHistoryCalculator historyCalculator)
                {
                    foreach (var stage in historyCalculator.InputData.ConstructionStages)
                    {
                        stage.InputData.Primitives.Clear();
                    }
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(ICalculator), calculator));
                }
            }
        }

        public static void RemoveAction(ICrossSectionRepository repository, IForceAction action)
        {
            foreach (var calculator in repository.Calculators)
            {
                if (calculator is IForceCalculator forceCalculator)
                {
                    forceCalculator.InputData.ForceActions.Remove(action);
                }
                else if (calculator is ICrackCalculator crackCalculator)
                {
                    crackCalculator.InputData.ForceActions.Remove(action);
                }
                else if (calculator is ILimitCurvesCalculator)
                {
                    // Limit-curve calculators do not keep repository action references.
                }
                else if (calculator is IValueDiagramCalculator valueDiagramCalculator)
                {
                    valueDiagramCalculator.InputData.ForceActions.Remove(action);
                }
                else if (calculator is ICurvatureCalculator curvatureCalculator)
                {
                    curvatureCalculator.InputData.ForceActions.Remove(action);
                }
                else if (calculator is IHistoryCalculator historyCalculator)
                {
                    foreach (var stage in historyCalculator.InputData.ConstructionStages)
                    {
                        if (ReferenceEquals(stage.InputData.ForceAction, action))
                        {
                            stage.InputData.ForceAction = null;
                        }
                    }
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(ICalculator), calculator));
                }
            }
        }

        public static void ClearActions(ICrossSectionRepository repository)
        {
            foreach (var calculator in repository.Calculators)
            {
                if (calculator is IForceCalculator forceCalculator)
                {
                    forceCalculator.InputData.ForceActions.Clear();
                }
                else if (calculator is ICrackCalculator crackCalculator)
                {
                    crackCalculator.InputData.ForceActions.Clear();
                }
                else if (calculator is ILimitCurvesCalculator)
                {
                    // Limit-curve calculators do not keep repository action references.
                }
                else if (calculator is IValueDiagramCalculator valueDiagramCalculator)
                {
                    valueDiagramCalculator.InputData.ForceActions.Clear();
                }
                else if (calculator is ICurvatureCalculator curvatureCalculator)
                {
                    curvatureCalculator.InputData.ForceActions.Clear();
                }
                else if (calculator is IHistoryCalculator historyCalculator)
                {
                    foreach (var stage in historyCalculator.InputData.ConstructionStages)
                    {
                        stage.InputData.ForceAction = null;
                    }
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(ICalculator), calculator));
                }
            }
        }
    }
}
