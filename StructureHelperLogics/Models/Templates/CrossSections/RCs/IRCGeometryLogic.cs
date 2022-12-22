using StructureHelper.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections.RCs
{
    public interface IRCGeometryLogic : ISectionGeometryLogic
    {
        IEnumerable<IHeadMaterial> HeadMaterials { get; set; }
    }
}
