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
        private ICloneStrategy<ICrossSection> cloneStrategy = new CrossSectionCloneStrategy();
        private IUpdateStrategy<ICrossSection> updateStrategy = new CrossSectionUpdateStrategy();
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
            //var newItem = new CrossSection();
            //updateStrategy.Update(newItem, this);
            //return newItem;
            return cloneStrategy.GetClone(this);
        }
    }
}
