using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.LoadHistory
{
    public interface IConstructionStage : ISaveable, IHasPrimitives, ICloneable
    {
        string Name { get; set; }
        int SubStepNumber { get; set; }
        IForceAction ForceAction { get; set; }
        double Duration { get; set; }
    }
}
