using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using System;
using System.Collections.Generic;

namespace DataAccess.DTOs
{
    public class CustomConcreteModelToDTOConvertStrategy
        : ConvertStrategy<CustomConcreteModelDTO, ICustomConcreteModel>
    {
        public CustomConcreteModelToDTOConvertStrategy()
        {
            ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>();
        }

        public override CustomConcreteModelDTO GetNewItem(ICustomConcreteModel source)
        {
            var newItem = new CustomConcreteModelDTO(source.Id);
            new CustomConcreteModelUpdateStrategy().Update(newItem, source);
            return newItem;
        }
    }
}
