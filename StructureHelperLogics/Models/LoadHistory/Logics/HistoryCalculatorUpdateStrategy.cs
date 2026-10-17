using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class HistoryCalculatorUpdateStrategy : IParentUpdateStrategy<IHistoryCalculator>
    {
        public bool UpdateChildren { get; set; } = true;

        public void Update(IHistoryCalculator targetObject, IHistoryCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            if (UpdateChildren == true)
            {
                targetObject.ConstructionStages.Clear();
                foreach (var item in sourceObject.ConstructionStages)
                {
                    var clone = item.Clone();
                }
            }
        }
    }
}
