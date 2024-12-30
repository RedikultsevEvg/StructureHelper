using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using System.Windows.Media;

namespace StructureHelperLogic.Models.Analyses
{
    public class CrossSectionNdmAnalysis : ICrossSectionNdmAnalysis
    {
        private CrossSectionNdmAnalysisUpdateStrategy updateStrategy = new();
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

        public CrossSectionNdmAnalysis(Guid id) : this (id, new VersionProcessor())
        {
            
        }

        public CrossSectionNdmAnalysis() : this(Guid.NewGuid(), new VersionProcessor())
        {
            CrossSection crossSection = new CrossSection();
            VersionProcessor.AddVersion(crossSection);
        }

        public object Clone()
        {
            CrossSectionNdmAnalysis newAnalysis = new();
            updateStrategy.Update(newAnalysis, this);
            var currentVersion = VersionProcessor.GetCurrentVersion().AnalysisVersion as ICloneable;
            ISaveable newCrossSection = currentVersion.Clone() as ISaveable;
            newAnalysis.VersionProcessor.Versions.Clear();
            newAnalysis.VersionProcessor.AddVersion(newCrossSection);
            return newAnalysis;
        }
    }
}
