using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorInputDataUpdateStrategy : IParentUpdateStrategy<ICurvatureCalculatorInputData>
    {
        private IUpdateStrategy<IDeflectionFactor> deflectionUpdateStrategy;
        private IUpdateStrategy<IDeflectionFactor> DeflectionUpdateStrategy => deflectionUpdateStrategy ??= new DeflectionFactorUpdateStrategy();

        public bool UpdateChildren { get; set; } = true;

        public void Update(ICurvatureCalculatorInputData targetObject, ICurvatureCalculatorInputData sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, nameof(sourceObject));
            CheckObject.ThrowIfNull(targetObject, nameof(targetObject));
            if (ReferenceEquals(targetObject, sourceObject))
                return;

            if (UpdateChildren == true)
            {
                CheckProperties(targetObject, sourceObject);
                targetObject.Primitives.Clear();
                targetObject.Primitives.AddRange(sourceObject.Primitives);
                targetObject.ForceActions.Clear();
                targetObject.ForceActions.AddRange(sourceObject.ForceActions);
            }
        }

        private static void CheckProperties(ICurvatureCalculatorInputData targetObject, ICurvatureCalculatorInputData sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject.Primitives);
            CheckObject.ThrowIfNull(targetObject.Primitives);
            CheckObject.ThrowIfNull(sourceObject.ForceActions);
            CheckObject.ThrowIfNull(targetObject.ForceActions);
        }
    }
}
