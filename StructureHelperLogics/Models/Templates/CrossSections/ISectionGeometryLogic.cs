using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections
{
    public interface ISectionGeometryLogic
    {
        IEnumerable<INdmPrimitive> GetNdmPrimitives();
    }
}
