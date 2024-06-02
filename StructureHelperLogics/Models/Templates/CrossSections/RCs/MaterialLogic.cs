using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperLogics.Models.Templates.CrossSections.RCs
{
    internal class MaterialLogic : IMaterialLogic
    {
        public IEnumerable<IHeadMaterial> GetHeadMaterials()
        {
            var result = new List<IHeadMaterial>();
            var concrete = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            concrete.Name = "Concrete";
            concrete.Color = (Color)ColorConverter.ConvertFromString("AliceBlue");
            result.Add(concrete);
            var reinforcement = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforcement400);
            reinforcement.Name = "Reinforcement";
            reinforcement.Color = (Color)ColorConverter.ConvertFromString("Red");
            result.Add(reinforcement);
            return result;
        }
    }
}
