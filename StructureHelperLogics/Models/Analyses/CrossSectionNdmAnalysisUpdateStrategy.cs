using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Services;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Analyses
{
    public class CrossSectionNdmAnalysisUpdateStrategy : IUpdateStrategy<ICrossSectionNdmAnalysis>
    {
        private IUpdateStrategy<IAnalysis> analysisUpdateStrategy;
        private IUpdateStrategy<ICrossSection> crossSectionUpdateStrategy;
        private IUpdateStrategy<IDateVersion> dateUpdateStrategy;

        public CrossSectionNdmAnalysisUpdateStrategy(IUpdateStrategy<IAnalysis> analysisUpdateStrategy,
            IUpdateStrategy<ICrossSection> crossSectionUpdateStrategy,
            IUpdateStrategy<IDateVersion> dateUpdateStrategy)
        {
            this.analysisUpdateStrategy = analysisUpdateStrategy;
            this.crossSectionUpdateStrategy = crossSectionUpdateStrategy;
            this.dateUpdateStrategy = dateUpdateStrategy;
        }

        public CrossSectionNdmAnalysisUpdateStrategy() : this(
            new AnalysisUpdateStrategy(),
            new CrossSectionUpdateStrategy(),
            new DateVersionUpdateStrategy())
        {
                
        }

        public void Update(ICrossSectionNdmAnalysis targetObject, ICrossSectionNdmAnalysis sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            analysisUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.VersionProcessor.Versions.Clear();
            foreach (var version in sourceObject.VersionProcessor.Versions)
            {
                if (version.AnalysisVersion is ICrossSection crossSection)
                {
                    updateVersion(targetObject, version, crossSection);
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(version.AnalysisVersion));
                }
            }
        }

        private void updateVersion(ICrossSectionNdmAnalysis targetObject, IDateVersion version, ICrossSection crossSection)
        {
            DateVersion newVersion = new();
            dateUpdateStrategy.Update(newVersion, version);
            CrossSection newCrossection = new();
            crossSectionUpdateStrategy.Update(newCrossection, crossSection);
            newVersion.AnalysisVersion = newCrossection;
            targetObject.VersionProcessor.Versions.Add(newVersion);
        }
    }
}
