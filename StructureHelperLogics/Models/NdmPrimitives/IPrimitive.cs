using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Entities;

namespace StructureHelperLogics.Models.NdmPrimitives
{
    public interface IPrimitive : ICenterShape
    {
        INdmPrimitive GetNdmPrimitive();
    }
}
