using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface IPointTriangulationLogicOptions : ITriangulationLogicOptions
    {
        IPoint2D Center { get; }
        double Area { get; }
    }
}
