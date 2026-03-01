using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;

namespace DataAccess.DTOs
{
    public class ConcreteFeaMaterialFromDTOConvertStrategy : ConvertStrategy<ConcreteFeaMaterial, ConcreteFeaMaterialDTO>
    {
        private IUpdateStrategy<IConcreteFeaMaterial> updateStrategy;
        private IConvertStrategy<ConcreteFeaCompression, ConcreteFeaCompressionDTO> compressionConvertStrategy;
        private IConvertStrategy<ConcreteFeaTension, ConcreteFeaTensionDTO> tensionConvertStrategy;

        public ConcreteFeaMaterialFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IConcreteFeaMaterial> UpdateStrategy => updateStrategy ??= new ConcreteFeaMaterialUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<ConcreteFeaCompression, ConcreteFeaCompressionDTO> CompressionConvertStrategy => compressionConvertStrategy ??= new ConcreteFeaCompressionFromDTOConvertStrategy(this);
        private IConvertStrategy<ConcreteFeaTension, ConcreteFeaTensionDTO> TensionConvertStrategy => tensionConvertStrategy ??= new ConcreteFeaTensionFromDTOConvertStrategy(this);

        public override ConcreteFeaMaterial GetNewItem(ConcreteFeaMaterialDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            if (source.CompressionProperties is ConcreteFeaCompressionDTO compressionDTO)
            {
                NewItem.CompressionProperties = CompressionConvertStrategy.Convert(compressionDTO);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.CompressionProperties));
            }
            if (source.TensionProperties is ConcreteFeaTensionDTO tensionDTO)
            {
                NewItem.TensionProperties = TensionConvertStrategy.Convert(tensionDTO);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.TensionProperties));
            }
            return NewItem;
        }
    }
}
