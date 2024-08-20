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
            var newInputData = new CrackCalculatorInputData();
            var checkLogic = new CheckCrackCalculatorInputDataLogic
            {
                InputData = newInputData
            };
            checkLogic.InputData = newInputData;
            var crackCalculator = new CrackCalculator(checkLogic, new CrackCalculatorUpdateStrategy(), null)
            {
                Name = "New Crack Calculator",
                TraceLogger = new ShiftTraceLogger()
            };
            crackCalculator.InputData = newInputData;
            calculators.Add(crackCalculator);
            return calculators;
        }
    }
}
