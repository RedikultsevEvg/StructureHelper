using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.LoadHistoryCalculator
{
    public interface IConstructionStage : ISaveable, IHasPrimitives
    {
        string Name { get; set; }
        int SubStepNumber { get; set; }
        IForceTuple ForceTuple { get; set; }
    }
}
