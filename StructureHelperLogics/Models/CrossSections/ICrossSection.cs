using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.WorkPlanes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.CrossSections
{
    public interface ICrossSection : ISaveable, ICloneable
    {
        ICrossSectionRepository SectionRepository { get; set; }
        IWorkPlaneProperty WorkPlaneProperty { get; set; }
    }
}
