using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculator : ICurvatureCalculator
    {
        private ICurvatureCalculatorResult result;
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public ICurvatureCalculatorInputData InputData { get; set; } = new CurvatureCalculatorInputData(Guid.NewGuid());
        public bool ShowTraceData { get; set; } = false;

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }


        public CurvatureCalculator(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            var updateStrategy = new CurvatureCalculatorUpdateStrategy();
            CurvatureCalculator newItem = new(Guid.NewGuid());
            updateStrategy.Update(newItem, this);
            return newItem;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
