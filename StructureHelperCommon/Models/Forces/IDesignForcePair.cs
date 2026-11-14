using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IDesignForcePair : IForceAction, ISelfCloneStrategy<IDesignForcePair>
    {
        LimitStates LimitState { get; set; }
        IForceTuple LongForceTuple { get; set; }
        IForceTuple FullForceTuple { get; set; }
    }
}
