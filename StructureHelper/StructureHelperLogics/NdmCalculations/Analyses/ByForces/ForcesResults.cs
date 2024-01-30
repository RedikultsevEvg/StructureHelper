using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForcesResults : IForcesResults
    {
        public bool IsValid { get; set; }
        public List<IForcesTupleResult> ForcesResultList { get; }
        public string Desctription { get; set; }

        public ForcesResults()
        {
            ForcesResultList = new List<IForcesTupleResult>();
        }
    }
}
