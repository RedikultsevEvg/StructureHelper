using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class PredicateFactory
    {
        public IEnumerable<INdm> Ndms { get; set; }
        public double My { get; set; }
        public bool GetResult(IPoint2D point)
        {
            var calculator = new ForceTupleCalculator();
            var tuple = new ForceTuple() { Nz = point.Y, Mx = point.X, My = My };
            var inputData = new ForceTupleInputData() { Tuple = tuple, NdmCollection = Ndms };
            calculator.InputData = inputData;
            calculator.Run();
            var result = calculator.Result;
            return !result.IsValid;
        }
    }
}
