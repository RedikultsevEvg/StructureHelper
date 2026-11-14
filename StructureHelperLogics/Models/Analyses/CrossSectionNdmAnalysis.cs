using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.WorkPlanes;
using StructureHelperLogics.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using System.Windows.Media;

namespace StructureHelperLogic.Models.Analyses
{
    public class CrossSectionNdmAnalysis : ICrossSectionNdmAnalysis
    {
        private IUpdateStrategy<ICrossSectionNdmAnalysis> updateStrategy;
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public IVersionProcessor VersionProcessor { get; set; }
        public string Comment { get; set; } = string.Empty;
        public Color Color { get; set; } = Color.FromRgb(128, 0, 0);

        public CrossSectionNdmAnalysis(Guid id, IVersionProcessor versionProcessor)
        {
            Id = id;
            VersionProcessor = versionProcessor;
        }

        public CrossSectionNdmAnalysis(Guid id) : this (id, new VersionProcessor(Guid.NewGuid()))
        {
            
        }

        public CrossSectionNdmAnalysis() : this(Guid.NewGuid(), new VersionProcessor(Guid.NewGuid()))
        {
            CrossSection crossSection = new();
            VersionProcessor.AddVersion(crossSection);
        }

        public IAnalysis GetClone(IAnalysis sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            updateStrategy ??= new CrossSectionNdmAnalysisUpdateStrategy();
            CrossSectionNdmAnalysis newAnalysis = new();
            updateStrategy.Update(newAnalysis, this);
            newAnalysis.VersionProcessor.Versions.Clear();
            var currentVersion = VersionProcessor.GetCurrentVersion().AnalysisVersion as ICrossSection;
            ICrossSection newCrossSection = GetCrossSectionClone(cloneMode, cloneDepth, currentVersion);
            newAnalysis.VersionProcessor.AddVersion(newCrossSection);
            return newAnalysis;
        }

        private static ICrossSection GetCrossSectionClone(CloneMode cloneMode, CloneDepth cloneDepth, ICrossSection? currentVersion)
        {
            var deepCloningStrategy = new DeepCloningStrategy();
            CrossSectionRepositoryCloneStrategy repositoryCloneStrategy = new(deepCloningStrategy);
            CrossSectionCloneStrategy cloneStrategy = new(repositoryCloneStrategy);
            ICrossSection newCrossSection = cloneStrategy.GetClone(currentVersion, cloneMode, cloneDepth);
            return newCrossSection;
        }
    }
}
