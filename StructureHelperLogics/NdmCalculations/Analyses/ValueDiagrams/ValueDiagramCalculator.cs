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
        public string Name { get; set; }
        public bool ShowTraceData { get; set; }

        public IResult Result => throw new NotImplementedException();

        public IShiftTraceLogger? TraceLogger { get; set; }

        public Guid Id { get; }

        public ValueDiagramCalculator(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
