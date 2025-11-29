using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    internal class CrackStrainLogic : ICrackStrainLogic
    {
        private IForceTupleServiceLogic forceTupleServiceLogic;
        private IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();
        public IForceTuple BeforeCrackingTuple { get; set; }
        public IForceTuple AfterCrackingTuple { get; set; }
        public double SofteningFactor { get; set; }

        public IForceTuple GetCrackedStrainTuple()
        {
            var strainTuple = ForceTupleServiceLogic.InterpolateTuples(BeforeCrackingTuple, AfterCrackingTuple, SofteningFactor) as StrainTuple;
            return strainTuple;
        }
    }
}
