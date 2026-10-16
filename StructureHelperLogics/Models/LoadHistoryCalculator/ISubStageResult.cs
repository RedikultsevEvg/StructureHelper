using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistoryCalculator
{
    internal interface ISubStageResult
    {
        IForceTuple ForceTuple { get; set; }
        IForceTuple StrainTuple { get; set; }
    }
}
