using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using System.Windows.Forms;

namespace DataAccess.DTOs
{
    public class ShapeToDTOConvertStrategy : ConvertStrategy<IShape, IShape>
    {
        private IConvertStrategy<RectangleShapeDTO, IRectangleShape> rectangleConvertStrategy;

        public ShapeToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override IShape GetNewItem(IShape source)
        {
            try
            {
                GetNewShape(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewShape(IShape source)
        {
            TraceLogger?.AddMessage($"Shape converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            if (source is IRectangleShape rectangle)
            {
                ProcessRectangle(rectangle);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source) + ": shape type";
                throw new StructureHelperException(errorString);
            }
            TraceLogger?.AddMessage($"Shape converting Id = {NewItem.Id} has been has been finished successfully", TraceLogStatuses.Debug);
        }

        private void ProcessRectangle(IRectangleShape rectangle)
        {
            TraceLogger?.AddMessage($"Shape is rectangle", TraceLogStatuses.Debug);
            rectangleConvertStrategy = new DictionaryConvertStrategy<RectangleShapeDTO, IRectangleShape>
                (this, new RectangleShapeToDTOConvertStrategy(this));
            NewItem = rectangleConvertStrategy.Convert(rectangle);
        }
    }
}
