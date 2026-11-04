using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Materials.Logics
{
    /// <summary>
    /// Updates the compression options of a custom concrete diagram.
    /// </summary>
    public class CustomConcreteCompressionOptionsUpdateStrategy
        : IUpdateStrategy<ICustomConcreteCompressionOptions>
    {
        public void Update(
            ICustomConcreteCompressionOptions targetObject,
            ICustomConcreteCompressionOptions sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject))
            {
                return;
            }

            targetObject.PeakStress = sourceObject.PeakStress;
            targetObject.PeakStrain = sourceObject.PeakStrain;
            targetObject.LimitStrain = sourceObject.LimitStrain;
            targetObject.DescendingScaleFactor = sourceObject.DescendingScaleFactor;
        }
    }
}
