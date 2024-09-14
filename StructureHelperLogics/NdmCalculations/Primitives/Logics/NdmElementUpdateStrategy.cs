using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Primitives.Logics
{
    public class NdmElementUpdateStrategy : IUpdateStrategy<INdmElement>
    {
        private readonly IUpdateStrategy<IForceTuple> tupleUpdateStrategy;

        public NdmElementUpdateStrategy(IUpdateStrategy<IForceTuple> tupleUpdateStrategy)
        {
            this.tupleUpdateStrategy = tupleUpdateStrategy;
        }

        public NdmElementUpdateStrategy() : this (new ForceTupleUpdateStrategy())
        {
            
        }

        /// <inheritdoc/>
        public void Update(INdmElement targetObject, INdmElement sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (sourceObject.HeadMaterial != null)
            {
                targetObject.HeadMaterial = sourceObject.HeadMaterial;
            }
            targetObject.Triangulate = sourceObject.Triangulate;
            tupleUpdateStrategy.Update(targetObject.UsersPrestrain, sourceObject.UsersPrestrain);
        }
    }
}
