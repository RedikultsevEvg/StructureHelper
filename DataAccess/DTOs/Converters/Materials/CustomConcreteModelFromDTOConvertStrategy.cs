using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using System;
using System.Collections.Generic;

namespace DataAccess.DTOs
{
    public class CustomConcreteModelFromDTOConvertStrategy
        : ConvertStrategy<CustomConcreteModel, CustomConcreteModelDTO>
    {
        public CustomConcreteModelFromDTOConvertStrategy()
        {
            ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>();
        }

        public override CustomConcreteModel GetNewItem(CustomConcreteModelDTO source)
        {
            var newItem = new CustomConcreteModel(source.Id);
            new CustomConcreteModelUpdateStrategy().Update(newItem, source);
            return newItem;
        }
    }
}
