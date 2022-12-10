using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.CrossSections
{
    public interface ICrossSection
    {
        ICrossSectionRepository SectionRepository { get; }
    }
}
