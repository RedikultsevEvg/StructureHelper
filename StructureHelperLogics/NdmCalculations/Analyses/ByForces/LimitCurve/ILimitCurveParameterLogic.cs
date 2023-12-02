using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface ILimitCurveParameterLogic : IHasActionByResult
    {
        IPoint2D CurrentPoint { get; set; }
        double GetParameter();
    }
}
