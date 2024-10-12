using System;
using System.CodeDom;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Forces
{
    public interface IDesignForceTuple : ISaveable, ICloneable
    {
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        IForceTuple ForceTuple { get; set; }
    }
}
