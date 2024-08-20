using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Materials
{
    public interface ILibMaterial : IHelperMaterial
    {
        ILibMaterialEntity MaterialEntity { get; set; }
        List<IMaterialSafetyFactor> SafetyFactors { get; set; }
        IMaterialLogic MaterialLogic { get; set; }
        List<IMaterialLogic> MaterialLogics { get; }
        (double Compressive, double Tensile) GetStrength(LimitStates limitState, CalcTerms calcTerm);
    }
}
