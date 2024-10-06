using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class HeadMaterialToDTOConvertStrategy : IConvertStrategy<HeadMaterialDTO, IHeadMaterial>
    {
        private IUpdateStrategy<IHeadMaterial> updateStrategy;
        private IConvertStrategy<IHelperMaterial, IHelperMaterial> convertStrategy;

        public HeadMaterialToDTOConvertStrategy(IUpdateStrategy<IHeadMaterial> updateStrategy,
            IConvertStrategy<IHelperMaterial, IHelperMaterial> convertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.convertStrategy = convertStrategy;
        }

        public HeadMaterialToDTOConvertStrategy() : this (
            new HeadMaterialUpdateStrategy(),
            new HelperMaterialToDTOConvertStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public HeadMaterialDTO Convert(IHeadMaterial source)
        {
            TraceLogger?.AddMessage($"Convert material Id={source.Id}, name is {source.Name}");
            HeadMaterialDTO newItem = new()
            {
                Id = source.Id
            };
            updateStrategy.Update(newItem, source);
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            var convertLogic = new DictionaryConvertStrategy<IHelperMaterial, IHelperMaterial>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = convertStrategy,
                TraceLogger = TraceLogger
            };
            newItem.HelperMaterial = convertLogic.Convert(source.HelperMaterial);
            return newItem;
        }
    }
}
