using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.Materials
{
    public interface IRebarSectionStrengthLogic : ILogic
    {
        IRebarSection RebarSection { get; set; }
        IShiftTraceLogger? TraceLogger { get; set; }

        double GetRebarMaxTensileForce();
    }
}