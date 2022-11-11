using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelper.Models.Materials;

namespace StructureHelperLogics.Models.Primitives
{
    public interface INdmPrimitive
    {
        ICenter Center { get; set; }
        IShape Shape { get; set; }
        IHeadMaterial HeadMaterial { get; }
        double NdmMaxSize { get; set; }
        int NdmMinDivision { get; set; }
        double PrestrainKx { get; set; }
        double PrestrainKy { get; set; }
        double PrestrainEpsZ { get; set; }
    }
}
