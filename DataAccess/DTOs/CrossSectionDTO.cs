using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CrossSectionDTO : ICrossSection
    {
        public ICrossSectionRepository SectionRepository { get; }

        public Guid Id { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
