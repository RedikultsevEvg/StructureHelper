using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceColumnPropertyUpdateStrategy : IUpdateStrategy<IColumnProperty>
    {
        public void Update(IColumnProperty targetObject, IColumnProperty sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.ColumnName = sourceObject.ColumnName;
            targetObject.ColumnIndex = sourceObject.ColumnIndex;
            targetObject.ColumnFactor = sourceObject.ColumnFactor;
        }
    }
}
