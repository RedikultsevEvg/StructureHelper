using System;
using System.Collections.Generic;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.Forces
{
    public interface IForceCombinationList : ICloneable
    {
        string Name { get; set; }
        bool SetInGravityCenter { get; set; }
        Point2D ForcePoint {get ;}
        List<IDesignForceTuple> DesignForces { get; }
    }
}
