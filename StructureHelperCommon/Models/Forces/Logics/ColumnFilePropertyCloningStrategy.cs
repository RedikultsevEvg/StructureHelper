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

        public IColumnFileProperty GetClone(IColumnFileProperty sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            if (updateStrategy is null)
            {
                updateStrategy = new ColumnFilePropertyUpdateStrategy();
            }
            ColumnFileProperty newItem = new(sourceObject.Name);
            updateStrategy.Update(newItem, sourceObject);
            return newItem;
        }
    }
}
