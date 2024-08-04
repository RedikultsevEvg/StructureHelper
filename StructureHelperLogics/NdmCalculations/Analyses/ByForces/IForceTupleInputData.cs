using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceTupleInputData : IInputData
    {
        IEnumerable<INdm> NdmCollection { get; set; }
        IForceTuple Tuple { get; set; }
        IAccuracy Accuracy { get; set; }
    }
}
