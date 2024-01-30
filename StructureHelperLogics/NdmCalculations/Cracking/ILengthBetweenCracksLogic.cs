using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ILengthBetweenCracksLogic
    {
        IEnumerable<INdm> NdmCollection { get; set; }
        IStrainMatrix StrainMatrix { get; set; }
        double GetLength();
    }
}
