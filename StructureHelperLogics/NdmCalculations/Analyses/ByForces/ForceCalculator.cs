using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculator : INdmCalculator
    {
        public string Name { get; set; }
        public List<IForceCombinationList> ForceCombinationLists { get; }
        public List<INdmPrimitive> NdmPrimitives { get; }
        public INdmResult Result { get; }
        public void Run()
        {
            throw new NotImplementedException();
        }

        public ForceCalculator()
        {
            ForceCombinationLists = new List<IForceCombinationList>();
            NdmPrimitives = new List<INdmPrimitive>();
        }
    }
}
