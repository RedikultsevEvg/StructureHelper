using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.WorkPlanes;
using StructureHelperLogics.Models.CrossSections;

namespace DataAccess.DTOs
{
    public class CrossSectionFromDTOConvertStrategy : IConvertStrategy<ICrossSection, ICrossSection>
    {
        private IConvertStrategy<ICrossSectionRepository, ICrossSectionRepository> repositoryConvertStrategy;
        private IConvertStrategy<WorkPlaneProperty, WorkPlanePropertyDTO> workPlaneConvertStrategy;

        public CrossSectionFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger? traceLogger)
        {
            ReferenceDictionary = referenceDictionary;
            TraceLogger = traceLogger;
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ICrossSection Convert(ICrossSection source)
        {
            InitializeStrategies();
            try
            {
                Check();
                return GetNewCrossSection(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private void InitializeStrategies()
        {
            repositoryConvertStrategy ??= new CrossSectionRepositoryFromDTOConvertStrategy(ReferenceDictionary, TraceLogger);
            workPlaneConvertStrategy ??= new WorkPlanePropertyFromDTOConvertStrategy(ReferenceDictionary,TraceLogger);
        }

        private ICrossSection GetNewCrossSection(ICrossSection source)
        {
            TraceLogger?.AddMessage("Cross-Section converting is started", TraceLogStatuses.Service);
            CrossSection newItem = new(source.Id);
            newItem.SectionRepository = GetNewCrossSectionRepository(source.SectionRepository);
            TraceLogger?.AddMessage("Cross-Section converting has been finished successfully", TraceLogStatuses.Service);
            if (source.WorkPlaneProperty is null)
            {
                TraceLogger?.AddMessage("Work plane properties is not found", TraceLogStatuses.Warning);
                newItem.WorkPlaneProperty = new WorkPlaneProperty(Guid.NewGuid());
                TraceLogger?.AddMessage("New work plane properties were generated", TraceLogStatuses.Debug);
            }
            else if (source.WorkPlaneProperty is WorkPlanePropertyDTO dto)
            {
                newItem.WorkPlaneProperty = workPlaneConvertStrategy.Convert(dto);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.WorkPlaneProperty));
            }
            return newItem;
        }

        private ICrossSectionRepository GetNewCrossSectionRepository(ICrossSectionRepository source)
        {
            ICrossSectionRepository newItem = repositoryConvertStrategy.Convert(source);
            TraceLogger?.AddMessage($"Object of type <<{newItem.GetType()}>> was obtained", TraceLogStatuses.Service);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<ICrossSection, ICrossSection>(this);
            checkLogic.Check();
        }
    }
}
