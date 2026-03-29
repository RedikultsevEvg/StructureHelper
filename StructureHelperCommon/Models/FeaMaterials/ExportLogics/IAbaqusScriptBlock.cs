using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IAbaqusScriptBlock
    {
        void Build(IAbaqusContext context);
    }
}
