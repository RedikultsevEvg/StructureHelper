using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class ConstructionStageInputData : IConstructionStageInputData
    {
        public IConstructionStage ConstructionStage { get; }
        private const int minSubSepNumber = 3;
        private const double minDuration = 0d;
        private int subStepNumber = 10;
        private double duration;

        public Guid Id {get; }
        public int SubStepNumber
        {
            get => subStepNumber;
            set
            {
                subStepNumber = Math.Max(value, minSubSepNumber);
            }
        }
        public List<INdmPrimitive> Primitives { get; } = [];
        public IForceAction ForceAction { get; set; }
        public double Duration
        {
            get => duration;
            set
            {
                duration = Math.Max(value, minDuration);
            }
        }


        public ConstructionStageInputData(Guid id, IConstructionStage constructionStage)
        {
            Id = id;
            ConstructionStage = constructionStage;
        }
    }
}
