using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials.ExportLogics
{
    public class ModelContext : IModelContext
    {
        public string ModelName { get; set; } = "New-model";
        public string ModelNameVar { get; set; } = "modelname";
        public string ForceFactorName { get; set; } = "forceFactor";
        public string LengthFactorName { get; set; } = "lengthFactor";
        public string StressFactorName { get; set; } = "stressFactor";
    }
}
