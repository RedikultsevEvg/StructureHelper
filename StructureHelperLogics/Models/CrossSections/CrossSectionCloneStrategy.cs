using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.WorkPlanes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionCloneStrategy : ICloneStrategy<ICrossSection>
    {
        private ICloneStrategy<ICrossSectionRepository> repositoryCloneStrategy;
        private IUpdateStrategy<IWorkPlaneProperty> workPlaneUpdateStrategy;

        public CrossSectionCloneStrategy(ICloneStrategy<ICrossSectionRepository> repositoryCloneStrategy,
            IUpdateStrategy<IWorkPlaneProperty> workPlaneUpdateStrategy)
        {
            this.repositoryCloneStrategy = repositoryCloneStrategy;
            this.workPlaneUpdateStrategy = workPlaneUpdateStrategy;
        }

        public CrossSectionCloneStrategy()  {   }

        public ICrossSection GetClone(ICrossSection sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            repositoryCloneStrategy ??= new CrossSectionRepositoryCloneStrategy(new DeepCloningStrategy());
            ICrossSectionRepository newRepository = repositoryCloneStrategy.GetClone(sourceObject.SectionRepository, cloningMode);
            var targetObject = new CrossSection(id)
            {
                SectionRepository = newRepository
            };
            CheckObject.ThrowIfNull(targetObject.WorkPlaneProperty);
            CheckObject.ThrowIfNull(sourceObject.WorkPlaneProperty);
            workPlaneUpdateStrategy ??= new WorkPlanePropertyUpdateStrategy();
            workPlaneUpdateStrategy.Update(targetObject.WorkPlaneProperty, sourceObject.WorkPlaneProperty);
            return targetObject;
        }
    }
}
