using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ColumnFilePropertyFromDTOConvertStrategy : ConvertStrategy<ColumnFileProperty, ColumnFilePropertyDTO>
    {
        private IUpdateStrategy<IColumnFileProperty> updateStrategy;
        public override ColumnFileProperty GetNewItem(ColumnFilePropertyDTO source)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            updateStrategy ??= new ColumnFilePropertyUpdateStrategy();
            ColumnFileProperty newItem = new(source.Id, source.Name);
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
