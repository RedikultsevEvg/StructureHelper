using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials
{
    internal static class MaterialLogicsFactory
    {
        public static List<IMaterialLogic> GetMaterialLogics()
        {
            var items = new List<IMaterialLogic>()
            {
                new ReinforcementByBuilderLogic(Guid.Parse("54c4fe40-8f82-4995-8930-81e65e97edb9"))
                {
                    MaterialType = MaterialTypes.Reinforcement,
                    Name="Bilinear",
                    DiagramType = DiagramType.Bilinear
                },
                new ReinforcementByBuilderLogic(Guid.Parse("c658b71d-13b1-458c-a1b0-c93d1324acad"))
                {
                    MaterialType = MaterialTypes.Reinforcement,
                    Name="Triplelinear",
                    DiagramType = DiagramType.TripleLinear
                },
                new ConcreteCurveLogic(Guid.Parse("b97e8168-76a1-4e24-ae98-9aa38edd1e9a"))
                {
                    MaterialType = MaterialTypes.Concrete,
                    Name = "Curve",
                    DiagramType = DiagramType.Curve
                },
                new SteelMaterialBuilderLogic(new Guid("C3BE4B92-DC61-43CF-A632-ADFC1AA57D8F"))
                {
                    MaterialType = MaterialTypes.Steel,
                    Name="Bilinear",
                    DiagramType = DiagramType.Bilinear
                },
                new SteelMaterialBuilderLogic(new Guid("7D6F9280-4DDF-43CE-8FBB-56FAE26BDA75"))
                {
                    MaterialType = MaterialTypes.Steel,
                    Name="Triplelinear",
                    DiagramType = DiagramType.TripleLinear
                },
            };
            return items;
        }
    }
}
