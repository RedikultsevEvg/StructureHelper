using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSection : ICrossSection
    {
        public ICrossSectionRepository SectionRepository { get; private set; }

        public CrossSection()
        {
            SectionRepository = new CrossSectionRepository();
        }
    }
}
