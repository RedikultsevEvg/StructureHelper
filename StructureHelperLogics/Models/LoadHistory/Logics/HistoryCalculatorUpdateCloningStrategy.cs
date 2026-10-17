using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class HistoryCalculatorUpdateCloningStrategy : IUpdateStrategy<IHistoryCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IConstructionStage> stageCloningStrategy;
        private IUpdateStrategy<IConstructionStage> StageCloningStrategy => stageCloningStrategy ??= new ConstructionStageUpdateCloningStrategy(cloningStrategy);

        public HistoryCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IHistoryCalculator targetObject, IHistoryCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
        }
    }
}
