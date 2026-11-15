using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Services;
using System;
using System.Windows.Media;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FeaMaterialAnalysis : IFeaMaterialAnalysis
    {
        private ICloneStrategy<IFeaMaterialRepository> cloneStrategy;
        private ICloneStrategy<IFeaMaterialRepository> CloneStrategy => cloneStrategy ??= new FeaMaterialRepositoryCloneStrategy();

        public string Name { get; set; }
        public string Tags { get; set; }
        public string Comment { get; set; }
        public Color Color { get; set; } = Color.FromRgb(128, 0, 0);
        public IVersionProcessor VersionProcessor { get; set; } = new VersionProcessor(Guid.NewGuid());

        public Guid Id { get; }

        public FeaMaterialAnalysis(Guid id)
        {
            this.Id = id;
            FeaMaterialRepository repository = new(Guid.NewGuid());
            VersionProcessor.AddVersion(repository);
        }

        public IAnalysis GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            FeaMaterialAnalysis analysisClone = new(id);
            var currentVersion = this.VersionProcessor.GetCurrentVersion().AnalysisVersion;
            if (currentVersion is not IFeaMaterialRepository feaMaterialRepository)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(currentVersion));
            }
            ISaveable repositoryClone = CloneStrategy.GetClone(feaMaterialRepository, cloneMode);
            analysisClone.VersionProcessor.AddVersion(repositoryClone);
            return analysisClone;
        }
    }
}
