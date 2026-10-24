using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class HistoryCalculatorInputDataUpdateStrategy : IParentUpdateStrategy<IHistoryCalculatorInputData>
    {
        public bool UpdateChildren { get; set; }

        public void Update(IHistoryCalculatorInputData targetObject, IHistoryCalculatorInputData sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
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
