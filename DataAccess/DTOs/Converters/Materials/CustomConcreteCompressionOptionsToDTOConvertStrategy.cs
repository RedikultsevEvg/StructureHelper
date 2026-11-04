using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using System;
using System.Collections.Generic;

namespace DataAccess.DTOs
{
    public class CustomConcreteCompressionOptionsToDTOConvertStrategy
        : ConvertStrategy<CustomConcreteCompressionOptionsDTO, ICustomConcreteCompressionOptions>
    {
        public CustomConcreteCompressionOptionsToDTOConvertStrategy()
        {
            ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>();
        }

        public override CustomConcreteCompressionOptionsDTO GetNewItem(ICustomConcreteCompressionOptions source)
        {
            var newItem = new CustomConcreteCompressionOptionsDTO(source.Id);
            new CustomConcreteCompressionOptionsUpdateStrategy().Update(newItem, source);
            return newItem;
        }
    }
}
