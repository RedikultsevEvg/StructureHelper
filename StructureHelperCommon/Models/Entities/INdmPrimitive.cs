using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.Entities
{
    public interface INdmPrimitive
    {
        ICenter Center { get; set; }
        IShape Shape { get; set; }
        IPrimitiveMaterial PrimitiveMaterial {get;set;}
        double NdmMaxSize { get; set; }
        int NdmMinDivision { get; set; }
        double PrestrainKx { get; set; }
        double PrestrainKy { get; set; }
        double PrestrainEpsZ { get; set; }
    }
}
