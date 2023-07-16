using LoaderCalculator.Data.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.MaterialBuilders
{
    internal interface IMaterialBuilder
    {
        IMaterialOption MaterialOption { get; set; }
        IMaterial GetMaterial();
    }
}
