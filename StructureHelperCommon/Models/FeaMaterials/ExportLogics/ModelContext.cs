using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials.ExportLogics
{
    public class ModelContext : IModelContext
    {
        public string ModelName { get; set; } = "New-model";
    }
}
