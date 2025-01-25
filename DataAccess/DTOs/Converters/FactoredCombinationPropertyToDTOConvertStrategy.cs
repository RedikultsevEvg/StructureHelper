using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class FactoredCombinationPropertyToDTOConvertStrategy : ConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty>
    {
        private IUpdateStrategy<IFactoredCombinationProperty> updateStrategy;

        public FactoredCombinationPropertyToDTOConvertStrategy(IUpdateStrategy<IFactoredCombinationProperty> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public FactoredCombinationPropertyToDTOConvertStrategy()
        {
            
        }
        public override FactoredCombinationPropertyDTO GetNewItem(IFactoredCombinationProperty source)
        {
            InitializeStrategies();
            TraceLogger?.AddMessage($"Force factored combination property Id={source.Id} converting has been started");
            FactoredCombinationPropertyDTO newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            TraceLogger?.AddMessage($"Force factored combination property Id={newItem.Id} converting has been finished");
            return newItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new FactoredCombinationPropertyUpdateStrategy();
        }
    }
}
