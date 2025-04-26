using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Functions;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    internal interface IFunctionMaterial : IHelperMaterial
    {
        double Modulus { get; set; }
        double CompressiveStrength { get; set; }
        double TensileStrength { get; set; }
        List<IMaterialSafetyFactor> SafetyFactors { get; }
        public List<MaterialSettings> MaterialSettings { get; set; }
        public IOneVariableFunction Function { get; set; }


    }
}
