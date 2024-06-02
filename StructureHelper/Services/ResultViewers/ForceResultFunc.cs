using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.ResultViewers
{
    public class ForceResultFunc : IResultFunc<Func<IStrainMatrix, INdm, double>>
    {
        public string Name { get; set; }
        public Func<IStrainMatrix, INdm, double> ResultFunction { get; set; }
        public string UnitName { get; set; }
        public double UnitFactor { get; set; }

        public ForceResultFunc()
        {
            UnitFactor = 1d;
        }
    }
}
