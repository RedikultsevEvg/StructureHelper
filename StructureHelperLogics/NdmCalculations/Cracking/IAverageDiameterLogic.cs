using LoaderCalculator.Data.Ndms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IAverageDiameterLogic
    {
        IEnumerable<RebarNdm> Rebars { get; set; }
        double GetAverageDiameter();
    }
}
