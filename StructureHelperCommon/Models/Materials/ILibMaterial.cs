using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using System.Collections.Generic;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Materials
{
    /// <summary>
    /// Implements logic for library material
    /// </summary>
    public interface ILibMaterial : IHelperMaterial, IMaterialStrength
    {
        ILibMaterialEntity MaterialEntity { get; set; }
        IMaterialLogic MaterialLogic { get; set; }
        List<IMaterialLogic> MaterialLogics { get; }
    }
}
