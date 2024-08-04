using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackForceCalculatorInputData : ICrackForceCalculatorInputData
    {
        public IForceTuple StartTuple { get; set; }
        public IForceTuple EndTuple { get; set; }
        public IEnumerable<INdm> CheckedNdmCollection { get; set; }
        public IEnumerable<INdm> SectionNdmCollection { get; set; }

        public CrackForceCalculatorInputData()
        {
            StartTuple = new ForceTuple();
            EndTuple = new ForceTuple();
        }
    }
}
