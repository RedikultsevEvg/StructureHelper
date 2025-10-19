using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class RectangleShapeToDTOConvertStrategy : ConvertStrategy<RectangleShapeDTO, IRectangleShape>
    {
        private IUpdateStrategy<IRectangleShape> updateStrategy;

        public RectangleShapeToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public RectangleShapeToDTOConvertStrategy()
        {
            
        }

        public override RectangleShapeDTO GetNewItem(IRectangleShape source)
        {
            try
            {
                GetNewRectangleShape(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewRectangleShape(IRectangleShape source)
        {
            TraceLogger?.AddMessage($"Rectangle shape converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            TraceLogger?.AddMessage($"Rectangle shape converting Id = {NewItem.Id} has been finished successfully", TraceLogStatuses.Debug);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new RectangleShapeUpdateStrategy();
        }
    }
}
