using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public class Point2DRange : IPoint2DRange
    {
        public Guid Id { get; }
        public IPoint2D StartPoint { get; set; } = new Point2D();
        public IPoint2D EndPoint { get; set; } = new Point2D();

        public Point2DRange(Guid id)
        {
            Id = id;
        }

        public IPoint2DRange GetClone(IPoint2DRange sourceObject, CloneMode cloningMode, CloneDepth cloneDepth)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            Point2DRange newItem = new(id);
            var updateStrategy = new Point2DRangeUpdateStrategy() { UpdateChildren = false};
            updateStrategy.Update(newItem, this);
            IPoint2D startPoint, endPoint;
            if (cloneDepth == CloneDepth.Deep)
            {
                startPoint = sourceObject.StartPoint.GetClone(sourceObject.StartPoint, cloningMode, CloneDepth.Deep);
                endPoint = sourceObject.StartPoint.GetClone(sourceObject.StartPoint, cloningMode, CloneDepth.Deep);
            }
            else if (cloneDepth == CloneDepth.Shallow) 
            {
                startPoint = sourceObject.StartPoint;
                endPoint = sourceObject.EndPoint;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cloneDepth));
            }
            newItem.StartPoint = startPoint;
            newItem.EndPoint = endPoint;
            return newItem;
        }
    }
}
