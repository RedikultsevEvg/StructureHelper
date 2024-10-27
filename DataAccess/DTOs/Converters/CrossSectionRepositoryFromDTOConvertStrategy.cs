using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CrossSectionRepositoryFromDTOConvertStrategy : ConvertStrategy<ICrossSectionRepository, ICrossSectionRepository>
    {
        private const string convertStarted = " converting is started";
        private const string convertFinished = " converting has been finished succesfully";
        private CrossSectionRepository newRepository;

        public override CrossSectionRepository GetNewItem(ICrossSectionRepository source)
        {
            TraceLogger?.AddMessage("Cross-Section repository" + convertStarted, TraceLogStatuses.Service);
            newRepository = new(source.Id);
            ProcessMaterials(source);
            ProcessActions(source);
            TraceLogger?.AddMessage("Cross-Section repository" + convertFinished, TraceLogStatuses.Service);
            return newRepository;
        }

        private void ProcessActions(ICrossSectionRepository source)
        {
            TraceLogger?.AddMessage("Actions"+ convertStarted, TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Actions" + convertFinished, TraceLogStatuses.Service);
            throw new NotImplementedException();
        }

        private void ProcessMaterials(IHasHeadMaterials source)
        {
            TraceLogger?.AddMessage("Materials" + convertStarted, TraceLogStatuses.Service);
            var convertStrategy = new HeadMaterialFromDTOConvertStrategy
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            var convertLogic = new DictionaryConvertStrategy<IHeadMaterial, IHeadMaterial>(this, convertStrategy);
            var updateStrategy = new HasMaterialFromDTOUpdateStrategy(convertLogic);
            updateStrategy.Update(newRepository, source);
            TraceLogger?.AddMessage("Materials" + convertFinished, TraceLogStatuses.Service);
        }
    }
}
