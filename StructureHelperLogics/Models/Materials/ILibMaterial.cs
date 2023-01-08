using StructureHelperCommon.Infrastructures.Enums;
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
        List<IMaterialSafetyFactor> SafetyFactors { get; }
    }
}
