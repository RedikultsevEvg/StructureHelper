using LoaderCalculator.Data.Ndms;
using System;

namespace StructureHelper.Services.ResultViewers
{
    public class ResultFuncByNdm : IResultFunc<Func<INdm, double>>
    {
        public string Name { get; set; } = string.Empty;

        public Func<INdm, double> ResultFunction { get; set; }

        public string UnitName { get; set; }

        public double UnitFactor { get; } = 1d;
    }
}
