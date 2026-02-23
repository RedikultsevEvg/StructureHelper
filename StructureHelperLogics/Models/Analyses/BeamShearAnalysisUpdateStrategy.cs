using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelperLogics.Models.Analyses
{
    public class BeamShearAnalysisUpdateStrategy : IUpdateStrategy<IBeamShearAnalysis>
    {
        private IUpdateStrategy<IAnalysis> analysisUpdateStrategy;
        private IUpdateStrategy<IBeamShear> beamShearUpdateStrategy;
        private IUpdateStrategy<IDateVersion> dateUpdateStrategy;
        private IUpdateStrategy<IAnalysis> AnalysisUpdateStrategy => analysisUpdateStrategy ??= new AnalysisUpdateStrategy();
        private IUpdateStrategy<IBeamShear> BeamShearUpdateStrategy => beamShearUpdateStrategy ??= new BeamShearUpdateStrategy();
        private IUpdateStrategy<IDateVersion> DateUpdateStrategy => dateUpdateStrategy ??= new DateVersionUpdateStrategy();

        public void Update(IBeamShearAnalysis targetObject, IBeamShearAnalysis sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            AnalysisUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.VersionProcessor.Versions.Clear();
            foreach (var version in sourceObject.VersionProcessor.Versions)
            {
                if (version.AnalysisVersion is IBeamShear beamShear)
                {
                    updateVersion(targetObject, version, beamShear);
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(version.AnalysisVersion));
                }
            }
        }

        private void updateVersion(IBeamShearAnalysis targetObject, IDateVersion version, IBeamShear beamShear)
        {
            DateVersion newVersion = new();
            DateUpdateStrategy.Update(newVersion, version);
            BeamShear newBeamShear = new(Guid.NewGuid());
            BeamShearUpdateStrategy.Update(newBeamShear, beamShear);
            newVersion.AnalysisVersion = newBeamShear;
            targetObject.VersionProcessor.Versions.Add(newVersion);
        }
    }
}
