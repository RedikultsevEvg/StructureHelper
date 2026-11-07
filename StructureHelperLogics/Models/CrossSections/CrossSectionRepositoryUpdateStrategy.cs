using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.CrossSections
{
    internal class CrossSectionRepositoryUpdateStrategy : IUpdateStrategy<ICrossSectionRepository>
    {
        public void Update(ICrossSectionRepository targetObject, ICrossSectionRepository sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject))
            {
                return;
            }

            var cloneStrategy = new CrossSectionRepositoryCloneStrategy(new DeepCloningStrategy());
            cloneStrategy.Populate(targetObject, sourceObject, CloningMode.NewEntity);
        }
    }
}
