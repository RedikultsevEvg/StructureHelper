using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IModelBlock : IAbaqusScriptBlock, IProvides<IModelContext>
    {
        string ModelNameVar { get; set; }
    }
}
