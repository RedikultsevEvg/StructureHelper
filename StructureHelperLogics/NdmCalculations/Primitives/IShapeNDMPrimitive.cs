using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface IShapeNdmPrimitive : INdmPrimitive, IHasDivisionSize
    {
        void SetShape(IShape shape);
    }
}
