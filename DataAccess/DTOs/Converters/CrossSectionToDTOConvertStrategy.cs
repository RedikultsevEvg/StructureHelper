using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.CrossSections;

namespace DataAccess.DTOs
{
    public class CrossSectionToDTOConvertStrategy : IConvertStrategy<CrossSectionDTO, ICrossSection>
    {
        private IUpdateStrategy<ICrossSection> updateStrategy; //don't use since CrossSection does not have any properties
        private IConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository> convertRepositoryStrategy;
        private DictionaryConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository> convertLogic;
        private ICheckConvertLogic<CrossSectionDTO, ICrossSection> checkLogic;

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public CrossSectionToDTOConvertStrategy(IUpdateStrategy<ICrossSection> updateStrategy,
            IConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository> convertRepositoryStrategy,
            ICheckConvertLogic<CrossSectionDTO, ICrossSection> checkLogic)
        {
            this.updateStrategy = updateStrategy;
            this.convertRepositoryStrategy = convertRepositoryStrategy;
            this.checkLogic = checkLogic;
        }

        public CrossSectionToDTOConvertStrategy() : this(
            new CrossSectionUpdateStrategy(),
            new CrossSectionRepositoryToDTOConvertStrategy(),
            new CheckConvertLogic<CrossSectionDTO, ICrossSection>())
        {
            
        }

        public CrossSectionDTO Convert(ICrossSection source)
        {
            Check();
            CrossSectionDTO newItem = new()
            {
                Id = source.Id
            };
            convertRepositoryStrategy.ReferenceDictionary = ReferenceDictionary;
            convertRepositoryStrategy.TraceLogger = TraceLogger;
            convertLogic = new DictionaryConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository>(this, convertRepositoryStrategy);
            newItem.SectionRepository = convertLogic.Convert(source.SectionRepository);
            return newItem;
        }

        private void Check()
        {
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
