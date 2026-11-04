using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using System;
using System.Collections.Generic;

namespace DataAccess.DTOs
{
    public class CustomConcreteCompressionOptionsFromDTOConvertStrategy
        : ConvertStrategy<CustomConcreteCompressionOptions, CustomConcreteCompressionOptionsDTO>
    {
        public CustomConcreteCompressionOptionsFromDTOConvertStrategy()
        {
            ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>();
        }

        public override CustomConcreteCompressionOptions GetNewItem(CustomConcreteCompressionOptionsDTO source)
        {
            var newItem = new CustomConcreteCompressionOptions(source.Id);
            new CustomConcreteCompressionOptionsUpdateStrategy().Update(newItem, source);
            return newItem;
        }
    }
}
