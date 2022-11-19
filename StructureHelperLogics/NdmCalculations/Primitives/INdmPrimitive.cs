using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelper.Models.Materials;
using System.Collections;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Materials;
using System.Collections.Generic;

namespace StructureHelperLogics.Models.Primitives
{
    public interface INdmPrimitive
    {
        string Name { get; set; }
        ICenter Center { get; set; }
        IShape Shape { get; set; }
        IHeadMaterial HeadMaterial { get; set; }
        double PrestrainKx { get; set; }
        double PrestrainKy { get; set; }
        double PrestrainEpsZ { get; set; }

        IEnumerable<INdm> GetNdms(IMaterial material);
    }
}
