using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Materials;

namespace StructureHelperLogics.NdmCalculations.Entities
{
    public class NdmPrimitive : INdmPrimitive
    {
        public ICenter Center { get; set; }
        public IShape Shape { get; set; }
        public IPrimitiveMaterial PrimitiveMaterial { get; set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
    }
}
