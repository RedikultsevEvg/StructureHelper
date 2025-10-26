using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Primitives.Logics
{
    public class NdmElementUpdateStrategy : IParentUpdateStrategy<INdmElement>
    {
        private readonly IUpdateStrategy<IForceTuple> tupleUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

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
            targetObject.Triangulate = sourceObject.Triangulate;
            tupleUpdateStrategy.Update(targetObject.UsersPrestrain, sourceObject.UsersPrestrain);
            tupleUpdateStrategy.Update(targetObject.AutoPrestrain, sourceObject.AutoPrestrain);
            if (UpdateChildren == true)
            {
                if (sourceObject.HeadMaterial != null)
                {
                    targetObject.HeadMaterial = sourceObject.HeadMaterial;
                }
            }
        }
    }
}
