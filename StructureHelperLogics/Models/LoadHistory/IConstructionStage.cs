using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.LoadHistory
{
    public interface IConstructionStage : ISaveable, ICloneable
    {
        string Name { get; set; }
        IConstructionStageInputData InputData { get; }
    }
}
