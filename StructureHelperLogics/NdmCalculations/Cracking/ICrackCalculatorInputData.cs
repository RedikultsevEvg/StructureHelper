using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackCalculatorInputData : IInputData, IHasPrimitives, IHasForceCombinations, ISaveable
    {
        List<IForceAction> ForceActions { get; }
        List<INdmPrimitive> Primitives { get; }
        IUserCrackInputData UserCrackInputData { get; set; }
    }
}