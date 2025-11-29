using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class DeflectionFactorUpdateStrategy : IParentUpdateStrategy<IDeflectionFactor>
    {
        public bool UpdateChildren { get; set; } = true;

        public void Update(IDeflectionFactor targetObject, IDeflectionFactor sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, nameof(sourceObject));
            CheckObject.ThrowIfNull(targetObject, nameof(targetObject));
            if (ReferenceEquals(targetObject, sourceObject))
                return;
            targetObject.SpanLength = sourceObject.SpanLength;
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(sourceObject.DeflectionFactors);
                CheckObject.ThrowIfNull(targetObject.DeflectionFactors);
                CheckObject.ThrowIfNull(sourceObject.MaxDeflections);
                CheckObject.ThrowIfNull(targetObject.MaxDeflections);
                targetObject.DeflectionFactors = sourceObject.DeflectionFactors.Clone() as IForceTuple;
                targetObject.MaxDeflections = sourceObject.MaxDeflections.Clone() as IForceTuple;
            }
        }
    }
}
