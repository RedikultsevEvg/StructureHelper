using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.BeamShears;
using System.Windows.Media;

namespace StructureHelperLogics.Models.Analyses
{
    /// <inheritdoc/>
    public class BeamShearAnalysis : IBeamShearAnalysis
    {
        private IUpdateStrategy<IBeamShearAnalysis> updateStrategy;
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;
        /// <inheritdoc/>
        public string Tags { get; set; } = string.Empty;
        /// <inheritdoc/>
        public string Comment { get; set; } = string.Empty;
        /// <inheritdoc/>
        public Color Color { get; set; } = Color.FromRgb(128, 0, 0);
        /// <inheritdoc/>
        public IVersionProcessor VersionProcessor { get; set; } = new VersionProcessor(Guid.NewGuid());
        public BeamShearAnalysis(Guid id)
        {
            Id = id;
            BeamShear beamShear = new(Guid.NewGuid());
            VersionProcessor.AddVersion(beamShear);
        }

        /// <inheritdoc/>
        public object Clone()
        {
            InitializeStrategies();
            BeamShearAnalysis newAnalysis = new(Guid.NewGuid());
            updateStrategy.Update(newAnalysis, this);
            newAnalysis.VersionProcessor.Versions.Clear();
            IBeamShear newVersion = GetCloneOfCurrentVersion();
            newAnalysis.VersionProcessor.AddVersion(newVersion);
            return newAnalysis;
        }

        private IBeamShear GetCloneOfCurrentVersion()
        {
            IBeamShear currentVersionOfSource = VersionProcessor.GetCurrentVersion().AnalysisVersion as IBeamShear;
            IBeamShear newVersion = currentVersionOfSource.Clone() as IBeamShear;
            return newVersion;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearAnalysisUpdateStrategy();
        }
    }
}
