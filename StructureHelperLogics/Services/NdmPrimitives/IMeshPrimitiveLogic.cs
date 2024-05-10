using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public interface IMeshPrimitiveLogic : ILogic
    {
        INdmPrimitive Primitive { get; set; }
        ITriangulationOptions TriangulationOptions { get; set; }
        List<INdm> MeshPrimitive();
    }
}
