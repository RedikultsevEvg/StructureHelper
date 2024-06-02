using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class StressStateFactorLogic : IStressStateFactorLogic
    {
        public ForceTuple ForceTuple { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetStressStateFactor()
        {
            if (ForceTuple.Nz > 0d)
            {
                TraceLogger.AddMessage($"Cross-section is tensioned since Nz = {ForceTuple.Nz}(N)");
                return 1.2d;
            }
            else
            {
                TraceLogger.AddMessage($"Cross-section is bent or compressed since Nz = {ForceTuple.Nz}(N)");
                return 1d;
            }
        }
    }
}
