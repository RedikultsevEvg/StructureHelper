using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Services;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;

namespace StructureHelperLogics.Models.Analyses
{
    public class CrossSectionNdmAnalysisUpdateStrategy : IParentUpdateStrategy<ICrossSectionNdmAnalysis>
    {
        private IUpdateStrategy<IAnalysis> analysisUpdateStrategy;
        private IUpdateStrategy<IAnalysis> AnalysisUpdateStrategy => analysisUpdateStrategy ??= new AnalysisUpdateStrategy();
        private IUpdateStrategy<ICrossSection> crossSectionUpdateStrategy;
        private IUpdateStrategy<ICrossSection> CrossSectionUpdateStrategy => crossSectionUpdateStrategy ??= new CrossSectionUpdateStrategy();
        private IUpdateStrategy<IDateVersion> dateUpdateStrategy;
        private IUpdateStrategy<IDateVersion> DateUpdateStrategy => dateUpdateStrategy ??= new DateVersionUpdateStrategy();

        public bool UpdateChildren { get; set; } = true;

        public CrossSectionNdmAnalysisUpdateStrategy(IUpdateStrategy<IAnalysis> analysisUpdateStrategy,
            IUpdateStrategy<ICrossSection> crossSectionUpdateStrategy,
            IUpdateStrategy<IDateVersion> dateUpdateStrategy)
        {
            this.analysisUpdateStrategy = analysisUpdateStrategy;
            this.crossSectionUpdateStrategy = crossSectionUpdateStrategy;
            this.dateUpdateStrategy = dateUpdateStrategy;
        }

        public CrossSectionNdmAnalysisUpdateStrategy() { }

        public void Update(ICrossSectionNdmAnalysis targetObject, ICrossSectionNdmAnalysis sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            AnalysisUpdateStrategy.Update(targetObject, sourceObject);
            if (UpdateChildren == true)
            {
                updateChildren(targetObject, sourceObject);
            }
        }

        private void updateChildren(ICrossSectionNdmAnalysis targetObject, ICrossSectionNdmAnalysis sourceObject)
        {
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
            DateUpdateStrategy.Update(newVersion, version);
            CrossSection newCrossection = new();
            CrossSectionUpdateStrategy.Update(newCrossection, crossSection);
            newVersion.AnalysisVersion = newCrossection;
            targetObject.VersionProcessor.Versions.Add(newVersion);
        }
    }
}
