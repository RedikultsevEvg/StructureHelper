using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;

namespace DataAccess.DTOs
{
    public class CustomConcreteHelperMaterialToDTOConvertStrategy
        : ConvertStrategy<CustomConcreteHelperMaterialDTO, ICustomConcreteHelperMaterial>
    {
        public CustomConcreteHelperMaterialToDTOConvertStrategy()
        {
            ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>();
        }

        public override CustomConcreteHelperMaterialDTO GetNewItem(ICustomConcreteHelperMaterial source)
        {
            var newItem = new CustomConcreteHelperMaterialDTO(source.Id);
            newItem.TensionForULS = source.TensionForULS;
            newItem.TensionForSLS = source.TensionForSLS;
            var safetyFactorStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(
                new MaterialSafetyFactorToDTOLogic());
            safetyFactorStrategy.Update(newItem, source);

            var modelConvertStrategy = new CustomConcreteModelToDTOConvertStrategy
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            foreach (ICustomConcreteModel model in source.MaterialModels)
            {
                newItem.MaterialModels.Add(modelConvertStrategy.Convert(model));
            }
            return newItem;
        }
    }
}
