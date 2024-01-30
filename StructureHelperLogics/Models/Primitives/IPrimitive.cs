using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Primitives
{
    public interface IPrimitive : ISaveable, ICloneable
    {
        string Name { get; set; }
        IPoint2D Center { get; }
        IShape Shape { get; }
        
        IEnumerable<INdmPrimitive> GetNdmPrimitives();
    }
}
