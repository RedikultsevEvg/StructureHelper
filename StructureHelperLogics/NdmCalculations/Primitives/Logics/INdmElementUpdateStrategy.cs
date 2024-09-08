using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Primitives.Logics
{
    public class INdmElementUpdateStrategy : IUpdateStrategy<INdmElement>
    {
        private readonly IUpdateStrategy<IPoint2D> point2DUpdateStrategy;
        private readonly IUpdateStrategy<IForceTuple> tupleUpdateStrategy;

        public INdmElementUpdateStrategy(IUpdateStrategy<IPoint2D> point2DUpdateStrategy,
            IUpdateStrategy<IForceTuple> tupleUpdateStrategy)
        {
            this.point2DUpdateStrategy = point2DUpdateStrategy;
            this.tupleUpdateStrategy = tupleUpdateStrategy;
        }

        public INdmElementUpdateStrategy() : this (
            new Point2DUpdateStrategy(),
            new ForceTupleUpdateStrategy())
        {
            
        }

        /// <inheritdoc/>
        public void Update(INdmElement targetObject, INdmElement sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }

            point2DUpdateStrategy.Update(targetObject.Center, sourceObject.Center);
            if (sourceObject.HeadMaterial != null)
            {
                targetObject.HeadMaterial = sourceObject.HeadMaterial;
            }
            targetObject.Triangulate = sourceObject.Triangulate;
            tupleUpdateStrategy.Update(targetObject.UsersPrestrain, sourceObject.UsersPrestrain);
        }
    }
}
