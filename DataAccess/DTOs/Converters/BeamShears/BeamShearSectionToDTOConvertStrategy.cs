using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearSectionToDTOConvertStrategy : ConvertStrategy<BeamShearSectionDTO, IBeamShearSection>
    {
        private IUpdateStrategy<IBeamShearSection> updateStrategy;
        private IConvertStrategy<IShape, IShape> shapeConvertStrategy;
        private ConcreteLibMaterialToDTOConvertStrategy concreteConvertStrategy;
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
            NewItem.Material = concreteConvertStrategy.Convert(source.Material);
            safetyFactorUpdateStrategy.Update(NewItem.Material, source.Material);
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
            safetyFactorUpdateStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorToDTOLogic());
        }
    }
}
