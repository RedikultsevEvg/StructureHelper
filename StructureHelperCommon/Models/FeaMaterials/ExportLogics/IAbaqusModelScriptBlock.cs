using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IAbaqusModelScriptBlock : IAbaqusScriptBlock
    {
        string ModelVariableName { get; set; }
    }
}
