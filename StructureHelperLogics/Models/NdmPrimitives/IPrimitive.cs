using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.NdmPrimitives
{
    public interface IPrimitive
    {
        ICenter Center { get;}
        IShape Shape { get;}
        INdmPrimitive GetNdmPrimitive();
    }
}
