using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class BeamShearSectionFromDTOConvertStrategy : ConvertStrategy<BeamShearSection, BeamShearSectionDTO>
    {
        private IUpdateStrategy<IBeamShearSection> updateStrategy;
        private IConvertStrategy<IShape, IShape> shapeConvertStrategy;
        private IConvertStrategy<ConcreteLibMaterial, ConcreteLibMaterialDTO> concreteConvertStrategy;
        private IConvertStrategy<ReinforcementLibMaterial, ReinforcementLibMaterialDTO> reinforcementConvertStrategy;
        private IUpdateStrategy<IHelperMaterial> safetyFactorUpdateStrategy;


        public BeamShearSectionFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override BeamShearSection GetNewItem(BeamShearSectionDTO source)
        {
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.Shape = shapeConvertStrategy.Convert(source.Shape);
            if (source.ConcreteMaterial is not ConcreteLibMaterialDTO concreteDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.ConcreteMaterial));
            }
            NewItem.ConcreteMaterial = concreteConvertStrategy.Convert(concreteDTO);
            safetyFactorUpdateStrategy.Update(NewItem.ConcreteMaterial, concreteDTO);
            if (source.ReinforcementMaterial is not ReinforcementLibMaterialDTO reinforcement)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.ReinforcementMaterial));
            }
            NewItem.ReinforcementMaterial = reinforcementConvertStrategy.Convert(reinforcement);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearSectionUpdateStrategy();
            shapeConvertStrategy = new DictionaryConvertStrategy<IShape, IShape>
                (this, new ShapeFromDTOConvertStrategy(this));
            concreteConvertStrategy = new ConcreteLibMaterialFromDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            reinforcementConvertStrategy = new ReinforcementLibMaterialFromDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            safetyFactorUpdateStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorsFromDTOLogic());
        }
    }
}
