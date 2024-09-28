using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class CrossSectionToDTOConvertStrategy : IConvertStrategy<CrossSectionDTO, ICrossSection>
    {
        private IUpdateStrategy<ICrossSection> updateStrategy;
        private IConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository> convertStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public CrossSectionToDTOConvertStrategy(IUpdateStrategy<ICrossSection> updateStrategy,
            IConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository> convertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.convertStrategy = convertStrategy;
        }

        public CrossSectionToDTOConvertStrategy() : this(
            new CrossSectionUpdateStrategy(),
            new CrossSectionRepositoryToDTOConvertStrategy())
        {
            
        }

        public CrossSectionDTO Convert(ICrossSection source)
        {
            Check();
            CrossSectionDTO newItem = new()
            {
                Id = source.Id
            };
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = convertStrategy,
                TraceLogger = TraceLogger
            };
            newItem.SectionRepository = convertLogic.Convert(source.SectionRepository);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<CrossSectionDTO, ICrossSection>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
