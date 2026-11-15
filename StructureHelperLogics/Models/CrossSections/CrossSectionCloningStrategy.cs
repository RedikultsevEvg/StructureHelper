using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionCloningStrategy : ICloneStrategy<ICrossSection>
    {
        private readonly ICloningStrategy cloningStrategy;


        private readonly ICloneStrategy<ICrossSectionRepository> repositoryCloneStrategy;
        private CloneMode cloneMode;
        private CloneDepth cloneDepth;
        private ICrossSection sourceObject;
        private CrossSection crossSection;

        public CrossSectionCloningStrategy(ICloningStrategy cloningStrategy) : this (cloningStrategy, new CrossSectionRepositoryCloneStrategy(cloningStrategy))
        {
        }

        public CrossSectionCloningStrategy(ICloningStrategy cloningStrategy, ICloneStrategy<ICrossSectionRepository> repositoryCloneStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.repositoryCloneStrategy = repositoryCloneStrategy;
        }

        public ICrossSection GetClone(ICrossSection sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject, "Source cross section");
            CheckObject.ThrowIfNull(sourceObject.SectionRepository, "Source cross-section repository");
            CheckObject.ThrowIfNull(cloningStrategy);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            if (cloneMode == CloneMode.NewEntity || cloneMode == CloneMode.PreserveIdentity)
            {
                this.cloneMode = cloneMode;
                this.cloneDepth = cloneDepth;
                this.sourceObject = sourceObject;
                GetCrossSection();
                return crossSection;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cloneMode) + $": Cross-section cloning, clone mode {cloneMode} is not correct");
            }
        }

        private void GetCrossSection()
        {
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            crossSection = new CrossSection(id);
            CrossSectionUpdateStrategy updateStrategy = new() { UpdateChildren = false};
            updateStrategy.Update(crossSection, sourceObject);
            crossSection.SectionRepository = cloningStrategy.Clone(sourceObject.SectionRepository, repositoryCloneStrategy, cloneMode);
            crossSection.WorkPlaneProperty = sourceObject.WorkPlaneProperty.GetClone(cloneMode, cloneDepth);
        }
    }
}
