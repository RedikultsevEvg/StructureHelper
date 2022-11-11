using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelper.Models.Materials;

namespace StructureHelperLogics.Models.Primitives
{
    public class NdmPrimitive : INdmPrimitive
    {
        private IHeadMaterial headMaterial;

        public ICenter Center { get; set; }
        public IShape Shape { get; set; }
        public IHeadMaterial HeadMaterial { get => headMaterial; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public double PrestrainKx { get; set; }
        public double PrestrainKy { get; set; }
        public double PrestrainEpsZ { get; set; }

        public NdmPrimitive(IHeadMaterial material)
        {
            headMaterial = material;
        }
    }
}
