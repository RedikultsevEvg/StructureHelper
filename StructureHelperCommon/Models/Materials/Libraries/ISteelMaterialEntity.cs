using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface ISteelMaterialEntity : ILibMaterialEntity
    {
        SteelDiagramPropertyType PropertyType { get; set; }
    }
}
