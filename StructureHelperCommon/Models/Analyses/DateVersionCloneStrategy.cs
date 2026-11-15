using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Analyses
{
    public class DateVersionCloneStrategy : ICloneStrategy<IDateVersion>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly ICloneStrategy<ISaveable>? analisysCloneStrategy;
        private DateVersion dateVersion;

        public DateVersionCloneStrategy(ICloningStrategy cloningStrategy, ICloneStrategy<ISaveable> analisysCloneStrategy = null)
        {
            this.cloningStrategy = cloningStrategy;
            this.analisysCloneStrategy = analisysCloneStrategy;
        }

        public IDateVersion GetClone(IDateVersion sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject, "Source date version");
            CheckObject.ThrowIfNull(sourceObject.AnalysisVersion, "Source date version analisis");
            CheckObject.ThrowIfNull(cloningStrategy);
            if (cloneMode == CloneMode.Reference) { return sourceObject;}
            if (cloneMode == CloneMode.NewEntity || cloneMode == CloneMode.PreserveIdentity)
            {
                GetVersionClone(sourceObject, cloneMode);
                return dateVersion;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cloneMode) + $": Date version cloning, clone mode {cloneMode} is not correct");
            }
        }

        private void GetVersionClone(IDateVersion sourceObject, CloneMode cloneMode)
        {
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            dateVersion = new DateVersion(id);
            new DateVersionUpdateStrategy().Update(dateVersion, sourceObject);
            dateVersion.AnalysisVersion = cloningStrategy.Clone(sourceObject.AnalysisVersion, analisysCloneStrategy, cloneMode);
        }
    }
}
