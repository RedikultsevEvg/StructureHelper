using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;

namespace StructureHelperLogic.Models.Analyses
{
    public class CrossSectionNdmAnalysis : ICrossSectionNdmAnalysis
    {
        private CrossSectionNdmAnalysisUpdateStrategy updateStrategy = new();
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public IVersionProcessor VersionProcessor { get; private set; }

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
            return newAnalysis;
        }
    }
}
