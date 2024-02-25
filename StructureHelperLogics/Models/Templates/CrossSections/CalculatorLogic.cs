using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace StructureHelperLogics.Models.Templates.CrossSections
{
    internal class CalculatorLogic : ICalculatorLogic
    {
        public IEnumerable<ICalculator> GetNdmCalculators()
        {
            var calculators = new List<ICalculator>
            {
                new ForceCalculator()
                {
                    Name = "New Force Calculator",
                    TraceLogger = new ShiftTraceLogger()
                }
            };
            return calculators;
        }
    }
}
