using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using System;
using System.Collections.Generic;

namespace DataAccess.DTOs
{
    public class CustomConcreteTensionOptionsFromDTOConvertStrategy
        : ConvertStrategy<CustomConcreteTensionOptions, CustomConcreteTensionOptionsDTO>
    {
        public CustomConcreteTensionOptionsFromDTOConvertStrategy()
        {
            ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>();
        }

        public override CustomConcreteTensionOptions GetNewItem(CustomConcreteTensionOptionsDTO source)
        {
            var newItem = new CustomConcreteTensionOptions(source.Id);
            new CustomConcreteTensionOptionsUpdateStrategy().Update(newItem, source);
            return newItem;
        }
    }
}
