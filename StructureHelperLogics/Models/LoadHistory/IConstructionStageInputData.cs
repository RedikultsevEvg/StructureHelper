using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public interface IConstructionStageInputData : ISaveable, IInputData, IHasPrimitives
    {
        IConstructionStage ConstructionStage { get; }
        int SubStepNumber { get; set; }
        IForceAction ForceAction { get; set; }
        double Duration { get; set; }
    }
}
