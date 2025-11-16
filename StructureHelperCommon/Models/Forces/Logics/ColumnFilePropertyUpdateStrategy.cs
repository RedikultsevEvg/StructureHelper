using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ColumnFilePropertyUpdateStrategy : IUpdateStrategy<IColumnFileProperty>
    {
        public void Update(IColumnFileProperty targetObject, IColumnFileProperty sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.SearchingName = sourceObject.SearchingName;
            targetObject.Index = sourceObject.Index;
            targetObject.Factor = sourceObject.Factor;
        }
    }
}
