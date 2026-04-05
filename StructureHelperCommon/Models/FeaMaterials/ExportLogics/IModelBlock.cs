using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IModelBlock : IAbaqusScriptBlock, IProvides<IModelContext>
    {
        string ModelName { get; set; }
        string ModelVariableName { get; set; }
        bool AddImport { get; set; }
    }
}
