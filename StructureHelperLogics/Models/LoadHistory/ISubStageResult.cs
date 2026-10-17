using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal interface ISubStageResult
    {
        IForceTuple ForceTuple { get; set; }
        IForceTuple StrainTuple { get; set; }
    }
}
