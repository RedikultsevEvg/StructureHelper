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
        private IUpdateStrategy<IColumnFileProperty> UpdateStrategy => updateStrategy ??= new ColumnFilePropertyUpdateStrategy();

        public ColumnFilePropertyCloningStrategy(IUpdateStrategy<IColumnFileProperty> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ColumnFilePropertyCloningStrategy()
        {
            
        }

        public IColumnFileProperty GetClone(IColumnFileProperty sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            ColumnFileProperty newItem = new(id, sourceObject.Name);
            UpdateStrategy.Update(newItem, sourceObject);
            return newItem;
        }
    }
}
