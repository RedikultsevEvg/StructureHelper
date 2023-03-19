using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IForceAction : IAction, ICloneable
    {
        bool SetInGravityCenter { get; set; }
        IPoint2D ForcePoint { get; set; }
        IForceCombinationList GetCombinations();
    }
}
