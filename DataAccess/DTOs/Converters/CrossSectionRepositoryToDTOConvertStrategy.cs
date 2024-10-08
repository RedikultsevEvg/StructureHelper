using DataAccess.DTOs.Converters;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CrossSectionRepositoryToDTOConvertStrategy : IConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository>
    {
        private IConvertStrategy<HeadMaterialDTO, IHeadMaterial> materialConvertStrategy;

        public CrossSectionRepositoryToDTOConvertStrategy(IConvertStrategy<HeadMaterialDTO, IHeadMaterial> materialConvertStrategy)
        {
            this.materialConvertStrategy = materialConvertStrategy;
        }

        public CrossSectionRepositoryToDTOConvertStrategy() : this(
            new HeadMaterialToDTOConvertStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public CrossSectionRepositoryDTO Convert(ICrossSectionRepository source)
        {
            Check();
            CrossSectionRepositoryDTO newItem = new()
            {
                Id = source.Id
            };
            List<HeadMaterialDTO> materials = ProcessMaterials(source);
            newItem.HeadMaterials.AddRange(materials);
            return newItem;
        }

        private List<HeadMaterialDTO> ProcessMaterials(ICrossSectionRepository source)
        {
            materialConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            materialConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<HeadMaterialDTO, IHeadMaterial>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = materialConvertStrategy,
                TraceLogger = TraceLogger
            };
            List<HeadMaterialDTO> materials = new();
            foreach (var item in source.HeadMaterials)
            {
                materials.Add(convertLogic.Convert(item));
            }

            return materials;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<CrossSectionRepositoryDTO, ICrossSectionRepository>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
