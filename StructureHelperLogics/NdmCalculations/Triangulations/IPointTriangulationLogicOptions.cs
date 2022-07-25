using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface IPointTriangulationLogicOptions : ITriangulationLogicOptions
    {
        ICenter Center { get; }
        double Area { get; }
    }
}
