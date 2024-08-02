using LoaderCalculator.Data.Ndms;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface IRebarPrimitive : IPointPrimitive, IHasHostPrimitive
    {
        Ndm GetConcreteNdm(ITriangulationOptions triangulationOptions);
        RebarNdm GetRebarNdm(ITriangulationOptions triangulationOptions);
    }
}