using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.MaterialBuilders;
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
        MaterialTypes MaterialType { get; set; }
        DiagramType DiagramType { get; set; }
    }
}
