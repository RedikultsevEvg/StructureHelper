using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    internal interface IConcreteBucklingCalculator : ILogicCalculator
    {
        IAccuracy Accuracy { get; set; }
    }
}
