using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Entities;

namespace StructureHelperLogics.Models.NdmPrimitives
{
    public abstract class PrimitiveBase<T> : IPrimitive where T : IShape
    {
        protected ICenter _center;
        protected T _shape;

        public ICenter Center => _center;
        public IShape Shape => _shape;

        public PrimitiveBase(ICenter center, T shape)
        {
            _center = center;
            _shape = shape;
        }

        public abstract INdmPrimitive GetNdmPrimitive();
    }
}
