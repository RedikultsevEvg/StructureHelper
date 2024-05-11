using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackedConcreteNdmLogic : ISectionCrackedLogic
    {
        public INdm ConcreteNdm { get; set; }
        public IForceTuple Tuple { get; set; }
        public IEnumerable<INdm> NdmCollection { get;set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool IsSectionCracked()
        {
            throw new NotImplementedException();
        }
    }
}
