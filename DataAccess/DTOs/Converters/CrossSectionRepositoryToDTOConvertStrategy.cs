using DataAccess.DTOs.Converters;
using NLog.Targets;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
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
            try
            {
                CrossSectionRepositoryDTO newItem = GetNewRepository(source);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private CrossSectionRepositoryDTO GetNewRepository(ICrossSectionRepository source)
        {
            CrossSectionRepositoryDTO newItem = new()
            {
                Id = source.Id
            };
            ProcessForceActions(newItem, source);
            List<IHeadMaterial> materials = ProcessMaterials(source);
            newItem.HeadMaterials.AddRange(materials);
            ProcessPrimitives(newItem, source);
            ProcessCalculators(newItem, source);
            return newItem;
        }

        private void ProcessCalculators(IHasCalculators target, IHasCalculators source)
        {
            HasCalculatorsToDTOUpdateStrategy updateStrategy = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            updateStrategy.Update(target, source);
        }

        private void ProcessPrimitives(IHasPrimitives target, IHasPrimitives source)
        {
            HasPrimitivesToDTOUpdateStrategy updateStrategy = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            updateStrategy.Update(target, source);
        }

        private void ProcessForceActions(IHasForceCombinations target, IHasForceCombinations source)
        {
            HasForceActionToDTOUpdateStrategy updateStrategy = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            updateStrategy.Update(target, source);
        }

        private List<IHeadMaterial> ProcessMaterials(ICrossSectionRepository source)
        {
            materialConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            materialConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<HeadMaterialDTO, IHeadMaterial>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = materialConvertStrategy,
                TraceLogger = TraceLogger
            };
            List<IHeadMaterial> materials = new();
            foreach (var item in source.HeadMaterials)
            {
                materials.Add(convertLogic.Convert(item));
            }

            return materials;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<CrossSectionRepositoryDTO, ICrossSectionRepository>(this);
            checkLogic.Check();
        }
    }
}
