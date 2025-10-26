using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs.Converters
{
    public class HasPrimitivesFromDTOUpdateStrategy : IUpdateStrategy<IHasPrimitives>
    {
        private IConvertStrategy<INdmPrimitive, INdmPrimitive> convertStrategy;

        public HasPrimitivesFromDTOUpdateStrategy(IConvertStrategy<INdmPrimitive, INdmPrimitive> convertStrategy)
        {
            this.convertStrategy = convertStrategy;
        }

        public void Update(IHasPrimitives targetObject, IHasPrimitives sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Primitives.Clear();
            foreach (var item in sourceObject.Primitives)
            {
                var newItem = convertStrategy.Convert(item);
                targetObject.Primitives.Add(newItem);
            }
        }
    }
}
