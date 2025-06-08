using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.WorkPlanes;
using StructureHelperLogics.Models.CrossSections;

namespace DataAccess.DTOs
{
    public class CrossSectionToDTOConvertStrategy : ConvertStrategy<CrossSectionDTO, ICrossSection>
    {
        private IUpdateStrategy<ICrossSection> updateStrategy; //don't use since CrossSection does not have any properties
        private IConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository> convertRepositoryStrategy;
        private DictionaryConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository> convertLogic;
        private ICheckConvertLogic<CrossSectionDTO, ICrossSection> checkLogic;
        private IConvertStrategy<WorkPlanePropertyDTO, IWorkPlaneProperty> workPlanePropertyConvertStrategy;

        public CrossSectionToDTOConvertStrategy(IUpdateStrategy<ICrossSection> updateStrategy,
            IConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository> convertRepositoryStrategy,
            ICheckConvertLogic<CrossSectionDTO, ICrossSection> checkLogic,
            IConvertStrategy<WorkPlanePropertyDTO, IWorkPlaneProperty> workPlanePropertyConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.convertRepositoryStrategy = convertRepositoryStrategy;
            this.checkLogic = checkLogic;
            this.workPlanePropertyConvertStrategy = workPlanePropertyConvertStrategy;
        }

        public CrossSectionToDTOConvertStrategy() {   }


        private void Check()
        {
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }

        public override CrossSectionDTO GetNewItem(ICrossSection source)
        {
            InitializeStrategies();
            try
            {
                GetNewItemBySource(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new CrossSectionUpdateStrategy();
            convertRepositoryStrategy ??= new CrossSectionRepositoryToDTOConvertStrategy(ReferenceDictionary, TraceLogger);
            checkLogic ??= new CheckConvertLogic<CrossSectionDTO, ICrossSection>();
            workPlanePropertyConvertStrategy ??= new WorkPlanePropertyToDTOConvertStrategy(ReferenceDictionary, TraceLogger);
        }

        private void GetNewItemBySource(ICrossSection source)
        {
            Check();
            NewItem = new()
            {
                Id = source.Id
            };
            convertLogic = new DictionaryConvertStrategy<CrossSectionRepositoryDTO, ICrossSectionRepository>(this, convertRepositoryStrategy);
            NewItem.SectionRepository = convertLogic.Convert(source.SectionRepository);
            NewItem.WorkPlaneProperty = workPlanePropertyConvertStrategy.Convert(source.WorkPlaneProperty);
        }
    }
}
