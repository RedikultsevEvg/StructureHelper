using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class ReinforcementMaterialEntity : IReinforcementMaterialEntity
    {
        public CodeTypes CodeType { get; set; }
        public string Name { get; set; }
        public double MainStrength { get; set; }
    }
}
