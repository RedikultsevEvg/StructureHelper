using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Analyses
{
    public class BeamShearAnalysisUpdateStrategy : IUpdateStrategy<IBeamShearAnalysis>
    {
        private IUpdateStrategy<IAnalysis> analysisUpdateStrategy;
        private IUpdateStrategy<IBeamShear> beamShearUpdateStrategy;
        private IUpdateStrategy<IDateVersion> dateUpdateStrategy;

        public void Update(IBeamShearAnalysis targetObject, IBeamShearAnalysis sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            InitialzeStrategies();
            analysisUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.VersionProcessor.Versions.Clear();
            foreach (var version in sourceObject.VersionProcessor.Versions)
            {
                if (version is IBeamShear beamShear)
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
            dateUpdateStrategy.Update(newVersion, version);
            BeamShear newBeamShear = new(Guid.NewGuid());
            beamShearUpdateStrategy.Update(newBeamShear, beamShear);
            newVersion.AnalysisVersion = newBeamShear;
            targetObject.VersionProcessor.Versions.Add(newVersion);
        }

        private void InitialzeStrategies()
        {
            analysisUpdateStrategy ??= new AnalysisUpdateStrategy();
            beamShearUpdateStrategy ??= new BeamShearUpdateStrategy();
            dateUpdateStrategy ??= new DateVersionUpdateStrategy();
        }
    }
}
