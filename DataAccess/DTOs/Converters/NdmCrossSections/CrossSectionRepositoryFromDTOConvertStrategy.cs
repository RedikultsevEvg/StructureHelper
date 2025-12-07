using DataAccess.DTOs.Converters;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class CrossSectionRepositoryFromDTOConvertStrategy : ConvertStrategy<ICrossSectionRepository, ICrossSectionRepository>
    {
        private const string convertStarted = " converting is started";
        private const string convertFinished = " converting has been finished successfully";
        private CrossSectionRepository newRepository;
        private ICheckEntityLogic<ICrossSectionRepository> checkEntityLogic;
        private ICheckEntityLogic<ICrossSectionRepository> CheckEntityLogic => checkEntityLogic ??= new CrossSectionRepositoryCheckLogic() {Entity = oldRepository};

        private IProcessLogic<IHasForcesAndPrimitives> forcesAndPrimitivesLogic;
        private IProcessLogic<IHasForcesAndPrimitives> ForcesAndPrimitivesLogic => forcesAndPrimitivesLogic ??= new HasForcesAndPrimitivesProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
        private ICrossSectionRepository oldRepository;

        public CrossSectionRepositoryFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override CrossSectionRepository GetNewItem(ICrossSectionRepository source)
        {
            oldRepository = source;
            if (CheckEntityLogic.Check() == false)
            {
                TraceLogger.AddMessage(CheckEntityLogic.CheckResult, TraceLogStatuses.Error);
                TraceLogger.AddMessage("All calculators will be removed", TraceLogStatuses.Error);
                oldRepository.Calculators.Clear();
            }
            TraceLogger?.AddMessage("Cross-Section repository" + convertStarted);
            newRepository = new(source.Id);
            ProcessMaterials(source);
            ProcessForcesAndPrimitives(source);
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


        private void ProcessForcesAndPrimitives(IHasForcesAndPrimitives source)
        {
            ForcesAndPrimitivesLogic.Source = source;
            ForcesAndPrimitivesLogic.Target = newRepository;
            ForcesAndPrimitivesLogic.Process();
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
