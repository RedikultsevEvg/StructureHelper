using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.ResultViewers
{
    public class CrackResultFunc : IResultFunc<Func<IRebarCrackResult, double>>
    {
        public string Name { get; set; }

        public Func<IRebarCrackResult, double> ResultFunction { get; set; }

        public string UnitName { get; set; }

        public double UnitFactor { get; set; }
    }
}
