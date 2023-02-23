using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections
{
    internal class CalculatorLogic : ICalculatorLogic
    {
        public IEnumerable<INdmCalculator> GetNdmCalculators()
        {
            var calculators = new List<INdmCalculator>();
            calculators.Add(new ForceCalculator() { Name = "New Force Calculator"});
            return calculators;
        }
    }
}
