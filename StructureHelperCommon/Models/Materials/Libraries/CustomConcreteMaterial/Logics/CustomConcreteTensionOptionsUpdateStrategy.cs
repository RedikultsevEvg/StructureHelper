using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Materials
{
    /// <summary>
    /// Updates the tension options of a custom concrete diagram.
    /// </summary>
    public class CustomConcreteTensionOptionsUpdateStrategy
        : IUpdateStrategy<ICustomConcreteTensionOptions>
    {
        public void Update(
            ICustomConcreteTensionOptions targetObject,
            ICustomConcreteTensionOptions sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject))
            {
                return;
            }
            targetObject.Strength = sourceObject.Strength;
            targetObject.UltimateStrain = sourceObject.UltimateStrain;
            targetObject.LimitStrain = sourceObject.LimitStrain;
        }
    }
}
