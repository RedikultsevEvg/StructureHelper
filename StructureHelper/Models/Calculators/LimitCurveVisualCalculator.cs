using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Models.Calculators
{
    internal class LimitCurveVisualCalculator : ISaveable, ICalculator
    {
        private LimitCurvesResult result;

        public Guid Id { get; }
        public string Name { get; set; }

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
