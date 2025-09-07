using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface INamedAreaPoint
    {
        double Area { get; set; }
        string Name { get; set; }
        IPoint2D Point { get; set; }
    }
}