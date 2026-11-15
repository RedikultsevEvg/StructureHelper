using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Analyses
{
    public class VersionProcessorCloneStrategy : ICloneStrategy<IVersionProcessor>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly ICloneStrategy<IDateVersion> versionCloneStrategy;

        private VersionProcessor versionProcessor;
        private CloneMode cloneMode;
        private CloneDepth cloneDepth;

        public VersionProcessorCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public IVersionProcessor GetClone(IVersionProcessor sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject, "Source version processor");
            CheckObject.ThrowIfNull(sourceObject.Versions, "Source date versions");
            CheckObject.ThrowIfNull(cloningStrategy);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            if (cloneMode == CloneMode.NewEntity || cloneMode == CloneMode.PreserveIdentity)
            {
                this.cloneMode = cloneMode;
                this.cloneDepth = cloneDepth;
                GetVersionProcessor(sourceObject);
                return versionProcessor;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cloneMode) + $": Date version cloning, clone mode {cloneMode} is not correct");
            }
        }

        private IVersionProcessor GetVersionProcessor(IVersionProcessor sourceObject)
        {
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            versionProcessor = new VersionProcessor(id);
            foreach (var version in sourceObject.Versions)
            {
                IDateVersion dateVersion = cloningStrategy.Clone(version, versionCloneStrategy, cloneMode);
                versionProcessor.AddVersion(dateVersion);
            }
            return versionProcessor;
        }
    }
}
