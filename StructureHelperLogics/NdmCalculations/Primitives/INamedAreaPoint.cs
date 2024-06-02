using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface INamedAreaPoint
    {
        double Area { get; set; }
        string Name { get; set; }
        Point2D Point { get; set; }
    }
}