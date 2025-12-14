using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Codes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class SteelMaterialEntity : ISteelMaterialEntity
    {
        public Guid Id { get; }
        public CodeTypes CodeType { get; set; }
        public SteelDiagramPropertyType PropertyType { get; set; }

        public ICodeEntity Code { get; set; }

        public string Name { get; set; } = string.Empty;

        public double InitialModulus { get; set; }

        public double MainStrength { get; set; }

        public SteelMaterialEntity(Guid id)
        {
            Id = id;
        }
    }
}
