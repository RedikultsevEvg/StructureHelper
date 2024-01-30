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
    public class ForceTupleInputData : IForceTupleInputData
    {
        public IEnumerable<INdm> NdmCollection { get; set; }
        public IForceTuple Tuple { get; set; }
        public IAccuracy Accuracy { get; set; }
        public ForceTupleInputData()
        {
            Accuracy ??= new Accuracy() { IterationAccuracy = 0.01d, MaxIterationCount = 1000 };
        }
    }
}
