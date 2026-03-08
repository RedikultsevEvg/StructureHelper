using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class CdpPropertyToDTOConvertStrategy : ConvertStrategy<CdpPropertyDTO, ICdpProperty>
    {
        private IUpdateStrategy<ICdpProperty> updateStrategy;

        public CdpPropertyToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<ICdpProperty> UpdateStrategy => updateStrategy ??= new CdpPropertyUpdateStrategy();
        public override CdpPropertyDTO GetNewItem(ICdpProperty source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
