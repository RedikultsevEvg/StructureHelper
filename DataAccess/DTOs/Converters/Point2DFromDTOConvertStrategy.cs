using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class Point2DFromDTOConvertStrategy : ConvertStrategy<Point2D, Point2DDTO>
    {
        private IUpdateStrategy<IPoint2D> updateStrategy;

        public Point2DFromDTOConvertStrategy(IUpdateStrategy<IPoint2D> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public Point2DFromDTOConvertStrategy() : this (new Point2DUpdateStrategy())
        {
            
        }

        public override Point2D GetNewItem(Point2DDTO source)
        {
            TraceLogger?.AddMessage("Point 2D converting has been started");
            try
            {
                Point2D newItem = GetNewItemBySource(source);
                TraceLogger?.AddMessage("Point 2D converting has been finished");
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage($"Logic: {LoggerStrings.LogicType(this)} made error: {ex.Message}", TraceLogStatuses.Error);
                throw;
            }
        }

        private Point2D GetNewItemBySource(Point2DDTO source)
        {
            Point2D newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
