using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface IHasHostPrimitive
    {
        INdmPrimitive? HostPrimitive { get; set; }
    }
}
