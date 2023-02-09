using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class ReinforcementMaterialEntity : IReinforcementMaterialEntity
    {
        public CodeTypes CodeType { get; set; }
        public string Name { get; set; }
        public double MainStrength { get; set; }
    }
}
