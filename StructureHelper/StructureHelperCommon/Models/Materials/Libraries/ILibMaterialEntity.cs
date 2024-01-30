using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface ILibMaterialEntity
    {
        CodeTypes CodeType { get; }
        string Name { get; }
        double MainStrength { get; }
    }
}
