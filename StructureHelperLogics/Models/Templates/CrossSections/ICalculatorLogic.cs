using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.Templates.CrossSections
{
    internal interface ICalculatorLogic
    {
        IEnumerable<ICalculator> GetNdmCalculators();
    }
}
