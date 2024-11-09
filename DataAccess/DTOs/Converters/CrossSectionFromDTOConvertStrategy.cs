using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CrossSectionFromDTOConvertStrategy : IConvertStrategy<ICrossSection, ICrossSection>
    {
        private readonly IConvertStrategy<ICrossSectionRepository, ICrossSectionRepository> convertStrategy;

        public CrossSectionFromDTOConvertStrategy(IConvertStrategy<ICrossSectionRepository, ICrossSectionRepository> convertStrategy)
        {
            this.convertStrategy = convertStrategy;
        }

        public CrossSectionFromDTOConvertStrategy() : this (new CrossSectionRepositoryFromDTOConvertStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ICrossSection Convert(ICrossSection source)
        {
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

        private ICrossSection GetNewCrossSection(ICrossSection source)
        {
            TraceLogger?.AddMessage("Cross-Section converting is started", TraceLogStatuses.Service);
            CrossSection newItem = new(source.Id);
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            newItem.SectionRepository = GetNewCrossSectionRepository(source.SectionRepository);
            TraceLogger?.AddMessage("Cross-Section converting has been finished successfully", TraceLogStatuses.Service);
            return newItem;
        }

        private ICrossSectionRepository GetNewCrossSectionRepository(ICrossSectionRepository source)
        {
            ICrossSectionRepository newItem = convertStrategy.Convert(source);
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
