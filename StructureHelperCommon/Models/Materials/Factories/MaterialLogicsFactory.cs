using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
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
                new ReinforcementByBuilderLogic() { MaterialType = MaterialTypes.Reinforcement, Name="Bilinear", DiagramType = DiagramType.Bilinear},
                new ReinforcementByBuilderLogic() { MaterialType = MaterialTypes.Reinforcement, Name="Triplelinear", DiagramType = DiagramType.TripleLinear},
                new ConcreteCurveLogic() { MaterialType = MaterialTypes.Concrete, Name = "Curve", DiagramType = DiagramType.Curve},
            };
            return items;
        }
    }
}
