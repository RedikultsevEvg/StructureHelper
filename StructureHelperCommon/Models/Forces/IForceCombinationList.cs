using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.Forces
{
    public interface IForceCombinationList : IForceAction, ICloneStrategy<IForceCombinationList>
    {
        List<IDesignForceTuple> DesignForces { get; set; }
    }
}
