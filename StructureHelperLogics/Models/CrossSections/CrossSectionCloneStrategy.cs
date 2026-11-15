using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.WorkPlanes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionCloneStrategy : ICloneStrategy<ICrossSection>
    {
        private ICloneStrategy<ICrossSectionRepository> repositoryCloneStrategy;
        private ICloneStrategy<ICrossSectionRepository> RepositoryCloneStrategy => repositoryCloneStrategy ??= new CrossSectionRepositoryCloneStrategy(new DeepCloningStrategy());
        private IUpdateStrategy<IWorkPlaneProperty> workPlaneUpdateStrategy;
        private IUpdateStrategy<IWorkPlaneProperty> WorkPlaneUpdateStrategy => workPlaneUpdateStrategy ??= new WorkPlanePropertyUpdateStrategy();

        public CrossSectionCloneStrategy(ICloneStrategy<ICrossSectionRepository> repositoryCloneStrategy)
        {
            this.repositoryCloneStrategy = repositoryCloneStrategy;
        }


        public ICrossSection GetClone(ICrossSection sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            var targetObject = new CrossSection(id);
            ICrossSectionRepository newRepository = RepositoryCloneStrategy.GetClone(sourceObject.SectionRepository, cloneMode, cloneDepth);
            newRepository.CrossSection = targetObject;
            targetObject.SectionRepository = newRepository;
            CheckObject.ThrowIfNull(targetObject.WorkPlaneProperty);
            CheckObject.ThrowIfNull(sourceObject.WorkPlaneProperty);
            WorkPlaneUpdateStrategy.Update(targetObject.WorkPlaneProperty, sourceObject.WorkPlaneProperty);
            return targetObject;
        }
    }
}
