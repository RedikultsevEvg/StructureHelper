using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ModelContext : IModelContext
    {
        public string ModelName { get; set; } = "New-model";
        public string ModelVaribleName { get; set; } = "modelname";
        public string ForceFactorName { get; set; } = "forceFactor";
        public string LengthFactorName { get; set; } = "lengthFactor";
        public string StressFactorName { get; set; } = "stressFactor";
    }
}
