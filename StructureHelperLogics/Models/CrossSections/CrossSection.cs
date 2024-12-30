using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSection : ICrossSection
    {
        public ICrossSectionRepository SectionRepository { get; set; } = new CrossSectionRepository();

        public Guid Id { get; private set; }

        public CrossSection(Guid id)
        {
            Id = id;
        }

        public CrossSection() : this(Guid.NewGuid())
        {
            
        }

        public object Clone()
        {
            ICloneStrategy<ICrossSection>  cloneStrategy = new CrossSectionCloneStrategy();
            return cloneStrategy.GetClone(this);
        }
    }
}
