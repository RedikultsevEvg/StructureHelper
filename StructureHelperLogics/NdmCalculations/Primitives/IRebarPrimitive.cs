using LoaderCalculator.Data.Ndms;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <summary>
    /// Geometry primitive of rebar (bar of reinforcement)
    /// </summary>
    public interface IRebarPrimitive : IPointPrimitive, IHasHostPrimitive
    {
        Ndm GetConcreteNdm(ITriangulationOptions triangulationOptions);
        RebarNdm GetRebarNdm(ITriangulationOptions triangulationOptions);
    }
}