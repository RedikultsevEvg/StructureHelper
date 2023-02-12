using StructureHelper.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public interface IHasHeadMaterials
    {
        List<IHeadMaterial> HeadMaterials { get; }
    }
}
