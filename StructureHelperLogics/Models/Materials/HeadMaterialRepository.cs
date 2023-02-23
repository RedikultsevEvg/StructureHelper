using StructureHelper.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class HeadMaterialRepository : IHeadMaterialRepository
    {
        public object Parent { get; private set; }

        public List<IHeadMaterial> HeadMaterials { get; set; }

        public HeadMaterialRepository()
        {
            HeadMaterials = new List<IHeadMaterial>();
        }

        public HeadMaterialRepository(object parent)
        {
            Parent = parent;
            HeadMaterials = new List<IHeadMaterial>();
        }

        public void RegisterParent(object obj)
        {
            Parent = obj;
        }
    }
}
