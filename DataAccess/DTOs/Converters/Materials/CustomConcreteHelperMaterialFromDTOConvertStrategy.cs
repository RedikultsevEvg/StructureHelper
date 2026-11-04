using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;

namespace DataAccess.DTOs
{
    public class CustomConcreteHelperMaterialFromDTOConvertStrategy
        : ConvertStrategy<CustomConcreteHelperMaterial, CustomConcreteHelperMaterialDTO>
    {
        public CustomConcreteHelperMaterialFromDTOConvertStrategy()
        {
            ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>();
        }

        public override CustomConcreteHelperMaterial GetNewItem(CustomConcreteHelperMaterialDTO source)
        {
            var newItem = new CustomConcreteHelperMaterial(source.Id);
            newItem.TensionForULS = source.TensionForULS;
            newItem.TensionForSLS = source.TensionForSLS;
            var safetyFactorStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(
                new MaterialSafetyFactorsFromDTOLogic());
            safetyFactorStrategy.Update(newItem, source);

            var modelConvertStrategy = new CustomConcreteModelFromDTOConvertStrategy
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            foreach (ICustomConcreteModel model in source.MaterialModels)
            {
                newItem.MaterialModels.Add(modelConvertStrategy.Convert(model as CustomConcreteModelDTO
                    ?? throw new InvalidOperationException(
                        $"{nameof(ICustomConcreteModel)} in {nameof(CustomConcreteHelperMaterialDTO)} must be {nameof(CustomConcreteModelDTO)}")));
            }
            return newItem;
        }
    }
}
