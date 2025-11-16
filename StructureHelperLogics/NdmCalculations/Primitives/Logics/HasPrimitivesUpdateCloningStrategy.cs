using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Primitives.Logics
{
    /// <summary>
    /// Creates deep copy of internal elements of object which has primitives
    /// </summary>
    public class HasPrimitivesUpdateCloningStrategy : IUpdateStrategy<IHasPrimitives>
    {
        private ICloningStrategy cloningStrategy;

        public HasPrimitivesUpdateCloningStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IHasPrimitives targetObject, IHasPrimitives sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Primitives.Clear();
            foreach (var primitive in sourceObject.Primitives)
            {
                ProcessPrimitive(targetObject, primitive);
            }
        }

        private void ProcessPrimitive(IHasPrimitives targetObject, INdmPrimitive primitive)
        {
            var newPrimitive = cloningStrategy.Clone(primitive);
            if (primitive.NdmElement.HeadMaterial is not null)
            {
                var material = cloningStrategy.Clone(primitive.NdmElement.HeadMaterial);
                newPrimitive.NdmElement.HeadMaterial = material;
            }
            targetObject.Primitives.Add(newPrimitive);
            if (primitive is IHasHostPrimitive hasHost)
            {
                if (hasHost.HostPrimitive is not null)
                {
                    INdmPrimitive hostPrimitive = cloningStrategy.Clone(hasHost.HostPrimitive);
                    (newPrimitive as IHasHostPrimitive).HostPrimitive = hostPrimitive;
                }
            }
        }
    }
}
