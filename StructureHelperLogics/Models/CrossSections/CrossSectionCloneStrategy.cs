using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionCloneStrategy : ICloneStrategy<ICrossSection>
    {
        private ICloneStrategy<ICrossSectionRepository> repositoryCloneStrategy;
        private CrossSection targetObject;

        public CrossSectionCloneStrategy(ICloneStrategy<ICrossSectionRepository> repositoryCloneStrategy)
        {
            this.repositoryCloneStrategy = repositoryCloneStrategy;
        }

        public CrossSectionCloneStrategy() : this (new CrossSectionRepositoryCloneStrategy())
        {
            
        }

        public ICrossSection GetClone(ICrossSection sourceObject)
        {
            ICrossSectionRepository newRepository = repositoryCloneStrategy.GetClone(sourceObject.SectionRepository);
            targetObject = new()
            {
                SectionRepository = newRepository
            };
            return targetObject;
        }
    }
}
