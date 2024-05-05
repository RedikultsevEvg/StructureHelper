using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperLogics.Models.Templates.CrossSections
{
    internal class CalculatorLogic : ICalculatorLogic
    {
        public IEnumerable<ICalculator> GetNdmCalculators()
        {
            var calculators = new List<ICalculator>();
            var forceCalculator = new ForceCalculator()
            {
                Name = "New Force Calculator",
                TraceLogger = new ShiftTraceLogger()
            };
            calculators.Add(forceCalculator);
            var crackCalculator = new CrackCalculator(new CrackInputData())
            {
                Name = "New Crack Calculator",
                TraceLogger = new ShiftTraceLogger()
            };
            calculators.Add(crackCalculator);
            return calculators;
        }
    }
}
