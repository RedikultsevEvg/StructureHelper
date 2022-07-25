using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.NdmPrimitives
{
    public interface IPrimitive : ICenterShape
    {
        INdmPrimitive GetNdmPrimitive();
    }
}
