using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IDesignForceTuple : ICloneable
    {
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        IForceTuple ForceTuple { get; set; }
    }
}
