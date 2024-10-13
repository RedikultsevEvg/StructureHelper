using DataAccess.DTOs.Converters;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
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
        private IConvertStrategy<ForceCombinationByFactorDTO, IForceCombinationByFactor> forceCombinationByFactorConvertStrategy;
        private IConvertStrategy<ForceCombinationListDTO, IForceCombinationList> forceCombinationListConvertStrategy;
        private IConvertStrategy<EllipseNdmPrimitiveDTO, IEllipsePrimitive> ellipseConvertStrategy = new EllipsePrimitiveDTOConvertStrategy();

        public CrossSectionRepositoryToDTOConvertStrategy(IConvertStrategy<HeadMaterialDTO, IHeadMaterial> materialConvertStrategy,
             IConvertStrategy<ForceCombinationByFactorDTO, IForceCombinationByFactor> forceCombinationByFactorConvertStrategy,
             IConvertStrategy<ForceCombinationListDTO, IForceCombinationList> forceCombinationListConvertStrategy)
        {
            this.materialConvertStrategy = materialConvertStrategy;
            this.forceCombinationByFactorConvertStrategy = forceCombinationByFactorConvertStrategy;
            this.forceCombinationListConvertStrategy = forceCombinationListConvertStrategy;
        }

        public CrossSectionRepositoryToDTOConvertStrategy() : this(
            new HeadMaterialToDTOConvertStrategy(),
            new ForceCombinationByFactorToDTOConvertStrategy(),
            new ForceCombinationListToDTOConvertStrategy())
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
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
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
            List<IForceAction> forceActions = ProcessForceActions(source);
            newItem.ForceActions.AddRange(forceActions);
            List<IHeadMaterial> materials = ProcessMaterials(source);
            newItem.HeadMaterials.AddRange(materials);
            List<INdmPrimitive> primitives = ProcessPrimitives(source);
            newItem.Primitives.AddRange(primitives);
            return newItem;
        }

        private List<INdmPrimitive> ProcessPrimitives(ICrossSectionRepository source)
        {
            List<INdmPrimitive> primitives = new();
            foreach (var item in source.Primitives)
            {
                if (item is IEllipsePrimitive ellipse)
                {
                    ellipseConvertStrategy.ReferenceDictionary = ReferenceDictionary;
                    ellipseConvertStrategy.TraceLogger = TraceLogger;
                    INdmPrimitive ndmPrimitive;
                    ndmPrimitive = ellipseConvertStrategy.Convert(ellipse);
                    primitives.Add(ndmPrimitive);
                }
            }
            return primitives;
        }

        private List<IForceAction> ProcessForceActions(ICrossSectionRepository source)
        {
            List<IForceAction> forceActions = new();
            foreach (var item in source.ForceActions)
            {         
                if (item is IForceCombinationByFactor forceCombinationByFactor)
                {
                    ForceCombinationByFactorDTO forceCombination = GetForceCombinationByFactor(forceCombinationByFactor);
                    forceActions.Add(forceCombination);
                }
                else if (item is IForceCombinationList forceCombinationList)
                {
                    ForceCombinationListDTO forceCombination = GetForceCombinationList(forceCombinationList);
                    forceActions.Add(forceCombination);
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(item));
                }
            }
            return forceActions;
        }

        private ForceCombinationListDTO GetForceCombinationList(IForceCombinationList forceCombinationList)
        {
            forceCombinationListConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            forceCombinationListConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ForceCombinationListDTO, IForceCombinationList>(this, forceCombinationListConvertStrategy);
            var forceCombination = convertLogic.Convert(forceCombinationList);
            return forceCombination;
        }

        private ForceCombinationByFactorDTO GetForceCombinationByFactor(IForceCombinationByFactor forceCombinationByFactor)
        {
            forceCombinationByFactorConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            forceCombinationByFactorConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ForceCombinationByFactorDTO, IForceCombinationByFactor>(this, forceCombinationByFactorConvertStrategy);
            var forceCombination = convertLogic.Convert(forceCombinationByFactor);
            return forceCombination;
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
