using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IForceCombinationList
    {
        string Name { get; set; }
        bool SetInGravityCenter { get; set; }
        Point2D ForcePoint {get ;}
        List<IDesignForceTuple> DesignForces { get; }
    }
}
