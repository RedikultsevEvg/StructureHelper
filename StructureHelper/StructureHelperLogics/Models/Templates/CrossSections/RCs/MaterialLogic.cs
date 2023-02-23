using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections.RCs
{
    internal class MaterialLogic : IMaterialLogic
    {
        public IEnumerable<IHeadMaterial> GetHeadMaterials()
        {
            var result = new List<IHeadMaterial>();
            var concrete = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40, ProgramSetting.CodeType);
            concrete.Name = "Concrete";
            result.Add(concrete);
            var reinforcement = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforecement400, ProgramSetting.CodeType);
            reinforcement.Name = "Reinforcement";
            result.Add(reinforcement);
            return result;
        }
    }
}
