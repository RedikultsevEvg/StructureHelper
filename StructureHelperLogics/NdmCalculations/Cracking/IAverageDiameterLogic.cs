using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IAverageDiameterLogic : ILogic
    {
        IEnumerable<RebarNdm> Rebars { get; set; }
        double GetAverageDiameter();
    }
}
