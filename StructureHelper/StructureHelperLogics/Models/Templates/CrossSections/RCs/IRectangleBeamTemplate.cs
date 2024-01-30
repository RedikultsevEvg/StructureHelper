using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.RCs
{
    public interface IRectangleBeamTemplate
    {
        IShape Shape { get; }
        double CoverGap { get; set; }
        double TopDiameter { get; set; }
        double BottomDiameter { get; set; }
        int WidthCount { get; set; }
        int HeightCount { get; set; }
    }
}
