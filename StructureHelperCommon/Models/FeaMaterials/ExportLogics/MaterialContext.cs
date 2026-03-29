using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class MaterialContext : IMaterialContext
    {
        public string MaterialName { get; set; } = "New material";
    }
}
