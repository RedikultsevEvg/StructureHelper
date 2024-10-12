using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class Point2DToDTOConvertStrategy : IConvertStrategy<Point2DDTO, IPoint2D>
    {
        private IUpdateStrategy<IPoint2D> updateStrategy;

        public Point2DToDTOConvertStrategy(IUpdateStrategy<IPoint2D> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public Point2DToDTOConvertStrategy() : this (new Point2DUpdateStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary {get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public Point2DDTO Convert(IPoint2D source)
        {
            try
            {
                Point2DDTO newItem = new() { Id = source.Id };
                updateStrategy.Update(newItem, source);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
            
        }
    }
}
