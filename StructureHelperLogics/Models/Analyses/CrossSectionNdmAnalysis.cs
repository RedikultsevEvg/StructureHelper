using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogic.Models.Analyses
{
    public class CrossSectionNdmAnalysis : IAnalysis
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public IVersionProcessor VersionProcessor { get; private set; }

        public CrossSectionNdmAnalysis(Guid Id, IVersionProcessor versionProcessor)
        {
            this.Id = Id;
            VersionProcessor = versionProcessor;
        }

        public CrossSectionNdmAnalysis() : this(new Guid(), new VersionProcessor())
        {
            CrossSection crossSection = new CrossSection();
            VersionProcessor.AddVersion(crossSection);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
