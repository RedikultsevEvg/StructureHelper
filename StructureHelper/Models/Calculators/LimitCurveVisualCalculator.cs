using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;

namespace StructureHelper.Models.Calculators
{
    internal class LimitCurveVisualCalculator : ISaveable, ICalculator
    {
        private LimitCurvesResult result;

        public Guid Id { get; }
        public string Name { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
