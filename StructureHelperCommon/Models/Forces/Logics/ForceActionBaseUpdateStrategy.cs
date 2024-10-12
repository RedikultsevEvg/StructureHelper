using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class ForceActionBaseUpdateStrategy : IUpdateStrategy<IForceAction>
    {
        private readonly IUpdateStrategy<IPoint2D> pointStrategy;

        public ForceActionBaseUpdateStrategy(IUpdateStrategy<IPoint2D> pointStrategy)
        {
            this.pointStrategy = pointStrategy;
        }

        public ForceActionBaseUpdateStrategy() : this(new Point2DUpdateStrategy())
        {
            
        }

        public void Update(IForceAction targetObject, IForceAction sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.SetInGravityCenter = sourceObject.SetInGravityCenter;
            pointStrategy.Update(targetObject.ForcePoint, sourceObject.ForcePoint);
        }
    }
}
