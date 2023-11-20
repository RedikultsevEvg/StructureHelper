using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials
{
    public interface IMaterialLogic
    {
        string Name { get; set; }
        IMaterialLogicOptions Options { get; set; }
        IMaterial GetLoaderMaterial();
    }
}
