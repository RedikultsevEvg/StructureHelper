using StructureHelper.Models.Materials;
using StructureHelperLogics.Models.Materials;
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
