using System;
using System.CodeDom;
using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Forces
{
    public interface IDesignForceTuple : ICloneable
    {
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        IForceTuple ForceTuple { get; set; }
    }
}
