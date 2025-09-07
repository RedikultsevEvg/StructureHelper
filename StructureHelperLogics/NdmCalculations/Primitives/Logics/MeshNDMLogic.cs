using LoaderCalculator.Data.Ndms;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface MeshNDMLogic
    {
        IEnumerable<INdm> GetNdms(ITriangulationOptions triangulation);
    }
}
