using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.NdmPrimitives
{
    public abstract class PrimitiveBase<T> : IPrimitive where T : IShape
    {
        protected ICenter _center;
        protected T _shape;

        public ICenter Center => _center;
        public IShape Shape => _shape;

        protected PrimitiveBase(ICenter center, T shape)
        {
            _center = center;
            _shape = shape;
        }

        public abstract INdmPrimitive GetNdmPrimitive();
    }
}
