using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic fo calculating of tensile area of RC crosssection
    /// </summary>
    public interface ITensileConcreteAreaLogic : ILogic
    {
        IEnumerable<INdm> NdmCollection { get; set; }
        IStrainMatrix StrainMatrix { get; set; }
        double GetTensileArea();
    }
}
