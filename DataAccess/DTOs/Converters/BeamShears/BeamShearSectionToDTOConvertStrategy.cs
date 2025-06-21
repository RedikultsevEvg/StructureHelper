using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class BeamShearSectionToDTOConvertStrategy : ConvertStrategy<BeamShearSectionDTO, IBeamShearSection>
    {
        private IUpdateStrategy<IBeamShearSection> updateStrategy;
        private IConvertStrategy<IShape, IShape> shapeConvertStrategy;
        private IConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial> concreteConvertStrategy;
        private ReinforcementLibMaterialToDTOConvertStrategy reinforcementConvertStrategy;
        private IUpdateStrategy<IHelperMaterial> safetyFactorUpdateStrategy;

        public BeamShearSectionToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override BeamShearSectionDTO GetNewItem(IBeamShearSection source)
        {
            try
            {
                GetNewSection(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewSection(IBeamShearSection source)
        {
            TraceLogger?.AddMessage($"Beam shear section converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.Shape = shapeConvertStrategy.Convert(source.Shape);
            NewItem.ConcreteMaterial = concreteConvertStrategy.Convert(source.ConcreteMaterial);
            safetyFactorUpdateStrategy.Update(NewItem.ConcreteMaterial, source.ConcreteMaterial);
            NewItem.ReinforcementMaterial = reinforcementConvertStrategy.Convert(source.ReinforcementMaterial);
            safetyFactorUpdateStrategy.Update(NewItem.ReinforcementMaterial, source.ReinforcementMaterial);
            TraceLogger?.AddMessage($"Beam shear section converting Id = {NewItem.Id} has been finished succesfully", TraceLogStatuses.Debug);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearSectionUpdateStrategy();
            shapeConvertStrategy = new DictionaryConvertStrategy<IShape, IShape>
                (this, new ShapeToDTOConvertStrategy(this));
            concreteConvertStrategy = new ConcreteLibMaterialToDTOConvertStrategy()
            { ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger};
            reinforcementConvertStrategy = new ReinforcementLibMaterialToDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            safetyFactorUpdateStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorToDTOLogic());
        }
    }
}
