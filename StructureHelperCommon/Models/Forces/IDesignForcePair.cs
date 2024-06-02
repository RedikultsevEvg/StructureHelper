using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IDesignForcePair : IForceAction
    {
        LimitStates LimitState { get; set; }
        IForceTuple LongForceTuple { get; set; }
        IForceTuple FullForceTuple { get; set; }
    }
}
