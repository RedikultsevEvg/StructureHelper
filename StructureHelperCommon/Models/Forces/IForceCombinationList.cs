using System;
using System.Collections.Generic;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.Forces
{
    public interface IForceCombinationList : IForceAction, ICloneable
    {
        List<IDesignForceTuple> DesignForces { get; }
    }
}
