using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthCalculator : ICalculator
    {
        CrackWidthCalculatorResult result;
        public string Name { get; set; }
        public ICrackWidthCalculatorInputData InputData { get; set; }
        public IResult Result => result;

        public void Run()
        {
            throw new NotImplementedException();
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
