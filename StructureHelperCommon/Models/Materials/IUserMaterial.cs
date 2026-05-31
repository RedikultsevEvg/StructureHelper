using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public interface IUserMaterial : IHelperMaterial
    {
        string? FilePath { get; set; }
    }
}
