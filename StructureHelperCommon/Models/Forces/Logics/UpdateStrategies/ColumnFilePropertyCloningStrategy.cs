using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ColumnFilePropertyCloningStrategy : ICloneStrategy<IColumnFileProperty>
    {
        private IUpdateStrategy<IColumnFileProperty> updateStrategy;

        public ColumnFilePropertyCloningStrategy(IUpdateStrategy<IColumnFileProperty> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ColumnFilePropertyCloningStrategy()
        {
            
        }

        public IColumnFileProperty GetClone(IColumnFileProperty sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            if (updateStrategy is null)
            {
                updateStrategy = new ColumnFilePropertyUpdateStrategy();
            }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            ColumnFileProperty newItem = new(id, sourceObject.Name);
            updateStrategy.Update(newItem, sourceObject);
            return newItem;
        }
    }
}
