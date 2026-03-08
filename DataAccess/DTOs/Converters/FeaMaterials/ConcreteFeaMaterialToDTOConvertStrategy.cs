using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class ConcreteFeaMaterialToDTOConvertStrategy : ConvertStrategy<ConcreteFeaMaterialDTO, IConcreteFeaMaterial>
    {
        private IUpdateStrategy<IConcreteFeaMaterial> updateStrategy;
        private IConvertStrategy<CdpPropertyDTO, ICdpProperty> cdpConvertStrategy;
        private IConvertStrategy<ConcreteFeaCompressionDTO, IConcreteFeaCompression> compressionConvertStrategy;
        private IConvertStrategy<ConcreteFeaTensionDTO, IConcreteFeaTension> tensionConvertStrategy;


        private IUpdateStrategy<IConcreteFeaMaterial> UpdateStrategy => updateStrategy ??= new ConcreteFeaMaterialUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<CdpPropertyDTO, ICdpProperty> CdpConvertStrategy => cdpConvertStrategy ??= new CdpPropertyToDTOConvertStrategy(this);
        private IConvertStrategy<ConcreteFeaCompressionDTO, IConcreteFeaCompression> CompressionConvertStrategy => compressionConvertStrategy ??= new ConcreteFeaCompressionToDTOConvertStrategy(this);
        private IConvertStrategy<ConcreteFeaTensionDTO, IConcreteFeaTension> TensionConvertStrategy => tensionConvertStrategy ??= new ConcreteFeaTensionToDTOConvertStrategy(this);
        
        public ConcreteFeaMaterialToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ConcreteFeaMaterialDTO GetNewItem(IConcreteFeaMaterial source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            NewItem.CdpProperty = CdpConvertStrategy.Convert(source.CdpProperty);
            NewItem.CompressionProperties = CompressionConvertStrategy.Convert(source.CompressionProperties);
            NewItem.TensionProperties = TensionConvertStrategy.Convert(source.TensionProperties);
            return NewItem;
        }
    }
}
