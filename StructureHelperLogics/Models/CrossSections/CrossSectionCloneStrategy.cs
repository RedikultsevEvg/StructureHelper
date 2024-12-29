using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;

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

        public CrossSectionCloneStrategy(ICloningStrategy cloningStrategy) : this (new CrossSectionRepositoryCloneStrategy(cloningStrategy))
        {
            
        }

        public CrossSectionCloneStrategy() : this (new DeepCloningStrategy())
        {
            
        }

        public ICrossSection GetClone(ICrossSection sourceObject)
        {
            var project = ProgramSetting.CurrentProject;
            ICrossSectionRepository newRepository = repositoryCloneStrategy.GetClone(sourceObject.SectionRepository);
            targetObject = new()
            {
                SectionRepository = newRepository
            };
            return targetObject;
        }
    }
}
