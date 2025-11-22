using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorInputDataUpdateStrategy : IParentUpdateStrategy<ICurvatureCalculatorInputData>
    {
        public bool UpdateChildren { get; set; } = true;

        public void Update(ICurvatureCalculatorInputData targetObject, ICurvatureCalculatorInputData sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, nameof(sourceObject));
            CheckObject.ThrowIfNull(targetObject, nameof(targetObject));
            if (ReferenceEquals(targetObject, sourceObject))
                return;
            targetObject.DeflectionFactor = sourceObject.DeflectionFactor;
            targetObject.SpanLength = sourceObject.SpanLength;
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(sourceObject.Primitives);
                CheckObject.ThrowIfNull(targetObject.Primitives);
                targetObject.Primitives.Clear();
                targetObject.Primitives.AddRange(sourceObject.Primitives);
                CheckObject.ThrowIfNull(sourceObject.ForceActions);
                CheckObject.ThrowIfNull(targetObject.ForceActions);
                targetObject.ForceActions.Clear();
                targetObject.ForceActions.AddRange(sourceObject.ForceActions);
            }
        }
    }
}
