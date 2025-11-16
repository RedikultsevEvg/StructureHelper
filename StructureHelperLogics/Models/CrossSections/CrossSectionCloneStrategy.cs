using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.WorkPlanes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionCloneStrategy : ICloneStrategy<ICrossSection>
    {
        private ICloneStrategy<ICrossSectionRepository> repositoryCloneStrategy;
        private IUpdateStrategy<IWorkPlaneProperty> workPlaneUpdateStrategy;
        private CrossSection targetObject;

        public CrossSectionCloneStrategy(ICloneStrategy<ICrossSectionRepository> repositoryCloneStrategy,
            IUpdateStrategy<IWorkPlaneProperty> workPlaneUpdateStrategy)
        {
            this.repositoryCloneStrategy = repositoryCloneStrategy;
            this.workPlaneUpdateStrategy = workPlaneUpdateStrategy;
        }

        public CrossSectionCloneStrategy()  {   }

        public ICrossSection GetClone(ICrossSection sourceObject)
        {
            repositoryCloneStrategy ??= new CrossSectionRepositoryCloneStrategy(new DeepCloningStrategy());
            ICrossSectionRepository newRepository = repositoryCloneStrategy.GetClone(sourceObject.SectionRepository);
            targetObject = new()
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
