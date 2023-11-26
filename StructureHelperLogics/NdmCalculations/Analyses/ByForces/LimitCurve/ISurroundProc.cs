using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface ISurroundProc
    {
        SurroundData SurroundData { get; set; }
        List<IPoint2D> GetPoints();
    }
}
