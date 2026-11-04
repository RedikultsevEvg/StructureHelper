using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using System;
using System.Collections.Generic;

namespace DataAccess.DTOs
{
    public class CustomConcreteTensionOptionsToDTOConvertStrategy
        : ConvertStrategy<CustomConcreteTensionOptionsDTO, ICustomConcreteTensionOptions>
    {
        public CustomConcreteTensionOptionsToDTOConvertStrategy()
        {
            ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>();
        }

        public override CustomConcreteTensionOptionsDTO GetNewItem(ICustomConcreteTensionOptions source)
        {
            var newItem = new CustomConcreteTensionOptionsDTO(source.Id);
            new CustomConcreteTensionOptionsUpdateStrategy().Update(newItem, source);
            return newItem;
        }
    }
}
