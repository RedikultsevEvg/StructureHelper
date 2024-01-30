using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class AverageDiameterLogic : IAverageDiameterLogic
    {
        public IEnumerable<RebarNdm> Rebars { get; set; }

        public double GetAverageDiameter()
        {
            Check();
            var rebarArea = Rebars
                .Sum(x => x.Area);
            var rebarCount = Rebars.Count();
            var averageArea = rebarArea / rebarCount;
            var diameter = Math.Sqrt(averageArea / Math.PI);
            return diameter;
        }
        private void Check()
        {
            if (!Rebars.Any())
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": rebars count must be greater then zero");
            }
        }
    }
}
