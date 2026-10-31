using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class HistoryCalculatorInputDataUpdateStrategy : IParentUpdateStrategy<IHistoryCalculatorInputData>
    {
        private IUpdateStrategy<IAccuracy> accuracyUpdateStrategy;
        private IUpdateStrategy<IAccuracy> AccuracyUpdateStrategy => accuracyUpdateStrategy ??= new AccuracyUpdateStrategy();
        public bool UpdateChildren { get; set; }

        public void Update(IHistoryCalculatorInputData targetObject, IHistoryCalculatorInputData sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.CheckStrainLimit = sourceObject.CheckStrainLimit;
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(sourceObject.Accuracy);
                CheckObject.ThrowIfNull(targetObject.Accuracy);
                AccuracyUpdateStrategy.Update(targetObject.Accuracy, sourceObject.Accuracy);
                targetObject.ConstructionStages.Clear();
                foreach (var item in sourceObject.ConstructionStages)
                {
                    var clone = item.Clone();
                }
            }
        }
    }
}
