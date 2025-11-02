using StructureHelperCommon.Services.Exports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface IGetPrimitivesLogic : IImportLogic
    {
        List<INdmPrimitive> Primitives { get; }
    }
}
