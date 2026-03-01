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
        private IConvertStrategy<ConcreteFeaCompressionDTO, IConcreteFeaCompression> compressionConvertStrategy;
        private IConvertStrategy<ConcreteFeaTensionDTO, IConcreteFeaTension> tensionConvertStrategy;

        public ConcreteFeaMaterialToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IConcreteFeaMaterial> UpdateStrategy => updateStrategy ??= new ConcreteFeaMaterialUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<ConcreteFeaCompressionDTO, IConcreteFeaCompression> CompressionConvertStrategy => compressionConvertStrategy ??= new ConcreteFeaCompressionToDTOConvertStrategy(this);
        private IConvertStrategy<ConcreteFeaTensionDTO, IConcreteFeaTension> TensionConvertStrategy => tensionConvertStrategy ??= new ConcreteFeaTensionToDTOConvertStrategy(this);

        public override ConcreteFeaMaterialDTO GetNewItem(IConcreteFeaMaterial source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            NewItem.CompressionProperties = CompressionConvertStrategy.Convert(source.CompressionProperties);
            NewItem.TensionProperties = TensionConvertStrategy.Convert(source.TensionProperties);
            return NewItem;
        }
    }
}
