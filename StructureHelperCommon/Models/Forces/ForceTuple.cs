using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceTuple : IForceTuple
    {
        public double Mx { get; set; }
        public double My { get; set; }
        public double Nz { get; set; }
        public double Qx { get; set; }
        public double Qy { get; set; }
        public double Mz { get; set; }

        public object Clone()
        {
            IForceTuple forceTuple = new ForceTuple() { Mx = Mx, My = My, Nz = Nz, Qx = Qx, Qy = Qy, Mz = Mz};
            return forceTuple;
        }
    }
}
