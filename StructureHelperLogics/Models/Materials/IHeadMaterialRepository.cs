using StructureHelper.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StructureHelperLogics.Models.Materials
{
    public interface IHeadMaterialRepository
    {
        object Parent { get; }
        List<IHeadMaterial> HeadMaterials { get; set; }
        List<ILibMaterial> LibMaterials { get; set; }
        void RegisterParent(object obj);

    }
}
