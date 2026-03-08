using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.ScriptExports
{
    public interface IScriptBuilder
    {
        string Build(IFeaMaterial material);
    }
}
