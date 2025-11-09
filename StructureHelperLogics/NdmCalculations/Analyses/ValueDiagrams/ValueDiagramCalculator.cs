using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramCalculator : IValueDiagramCalculator
    {
        private IValueDiagramCalculatorResult result;

        public Guid Id { get; }
        public string Name { get; set; }
        public bool ShowTraceData { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public IValueDiagramCalculatorInputData InputData { get; set; } = new ValueDiagramCalculatorInputData(Guid.NewGuid());

        public ValueDiagramCalculator(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            ValueDiagramCalculator newItem = new ValueDiagramCalculator(Guid.NewGuid());
            var updateLogic = new ValueDiagramCalculatorUpdateStrategy();
            updateLogic.Update(newItem, this);
            return newItem;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
