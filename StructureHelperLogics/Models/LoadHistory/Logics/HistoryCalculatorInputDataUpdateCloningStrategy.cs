using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory.Logics
{
    internal class HistoryCalculatorInputDataUpdateCloningStrategy : IUpdateStrategy<IHistoryCalculatorInputData>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IConstructionStage> stageCloningStrategy;

        public HistoryCalculatorInputDataUpdateCloningStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        private IUpdateStrategy<IConstructionStage> StageCloningStrategy => stageCloningStrategy ??= new ConstructionStageUpdateCloningStrategy(cloningStrategy);
        public void Update(IHistoryCalculatorInputData targetObject, IHistoryCalculatorInputData sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.ConstructionStages.Clear();
            foreach (var sourceStage in sourceObject.ConstructionStages)
            {
                var cloneStage = sourceStage.Clone() as IConstructionStage;
                StageCloningStrategy.Update(cloneStage, sourceStage);
                targetObject.ConstructionStages.Add(cloneStage);
            }
        }
    }
}
