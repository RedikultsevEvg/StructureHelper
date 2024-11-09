using DataAccess.DTOs.Converters;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
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
        private const string convertFinished = " converting has been finished successfully";
        private CrossSectionRepository newRepository;

        private IHasPrimitivesProcessLogic primitivesProcessLogic = new HasPrimitivesProcessLogic();
        private IHasForceActionsProcessLogic actionsProcessLogic = new HasForceActionsProcessLogic();

        public override CrossSectionRepository GetNewItem(ICrossSectionRepository source)
        {
            TraceLogger?.AddMessage("Cross-Section repository" + convertStarted);
            newRepository = new(source.Id);
            ProcessMaterials(source);
            ProcessActions(source);
            ProcessPrimitives(source);
            ProcessCalculators(source);
            TraceLogger?.AddMessage("Cross-Section repository" + convertFinished);
            return newRepository;
        }

        private void ProcessCalculators(ICrossSectionRepository source)
        {
            TraceLogger?.AddMessage("Calculators" + convertStarted);
            var convertStrategy = new CalculatorsFromDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            var convertLogic = new DictionaryConvertStrategy<ICalculator, ICalculator>(this, convertStrategy);
            var updateStrategy = new HasCalculatorsFromDTOUpdateStrategy(convertLogic);
            updateStrategy.Update(newRepository, source);
            TraceLogger?.AddMessage("Calculators" + convertFinished);
        }

        private void ProcessPrimitives(IHasPrimitives source)
        {
            primitivesProcessLogic.Source = source;
            primitivesProcessLogic.Target = newRepository;
            primitivesProcessLogic.ReferenceDictionary = ReferenceDictionary;
            primitivesProcessLogic.TraceLogger = TraceLogger;
            primitivesProcessLogic.Process();
        }

        private void ProcessActions(IHasForceActions source)
        {
            actionsProcessLogic.Source = source;
            actionsProcessLogic.Target = newRepository;
            actionsProcessLogic.ReferenceDictionary = ReferenceDictionary;
            actionsProcessLogic.TraceLogger = TraceLogger;
            actionsProcessLogic.Process();
        }

        private void ProcessMaterials(IHasHeadMaterials source)
        {
            TraceLogger?.AddMessage("Materials" + convertStarted);
            var convertStrategy = new HeadMaterialFromDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            var convertLogic = new DictionaryConvertStrategy<IHeadMaterial, IHeadMaterial>(this, convertStrategy);
            var updateStrategy = new HasMaterialFromDTOUpdateStrategy(convertLogic);
            updateStrategy.Update(newRepository, source);
            TraceLogger?.AddMessage("Materials" + convertFinished);
        }
    }
}
