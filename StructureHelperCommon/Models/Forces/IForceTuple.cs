using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IForceTuple
    {
        double Mx { get; set; }
        double My { get; set; }
        double Nz { get; set; }
        double Qx { get; set; }
        double Qy { get; set; }
        double Mz { get; set; }

    }
}
