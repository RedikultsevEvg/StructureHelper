using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ColumnPropertyCloningStrategy : ICloneStrategy<IColumnProperty>
    {
        private IUpdateStrategy<IColumnProperty> updateStrategy;

        public ColumnPropertyCloningStrategy(IUpdateStrategy<IColumnProperty> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ColumnPropertyCloningStrategy()
        {
            
        }

        public IColumnProperty GetClone(IColumnProperty sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            if (updateStrategy is null)
            {
                updateStrategy = new ColumnPropertyUpdateStrategy();
            }
            ColumnProperty newItem = new(sourceObject.Name);
            updateStrategy.Update(newItem, sourceObject);
            return newItem;
        }
    }
}
