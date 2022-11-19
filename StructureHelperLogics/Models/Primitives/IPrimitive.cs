using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
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
        ICenter Center { get; }
        IShape Shape { get; }
        
        IEnumerable<INdmPrimitive> GetNdmPrimitives();
    }
}
