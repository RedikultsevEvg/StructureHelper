using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public interface ICheckPrimitivesForMeshingLogic : ILogic
    {
        IEnumerable<INdmPrimitive> Primitives { get; set; }
        bool Check();
    }
}
