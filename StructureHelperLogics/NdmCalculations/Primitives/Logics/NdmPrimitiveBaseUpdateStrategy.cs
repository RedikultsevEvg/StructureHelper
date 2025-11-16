using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    internal class NdmPrimitiveBaseUpdateStrategy : IParentUpdateStrategy<INdmPrimitive>
    {
        private IUpdateStrategy<INdmElement> ndmElementUpdateStrategy;
        private IUpdateStrategy<IPoint2D> point2DUpdateStrategy;
        private IUpdateStrategy<IVisualProperty> visualPropsUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

        public void Update(INdmPrimitive targetObject, INdmPrimitive sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            targetObject.Name = sourceObject.Name;
            targetObject.RotationAngle = sourceObject.RotationAngle;
            targetObject.CrossSection = sourceObject.CrossSection;
            if (UpdateChildren == true)
            {
                point2DUpdateStrategy.Update(targetObject.Center, sourceObject.Center);
                visualPropsUpdateStrategy.Update(targetObject.VisualProperty, sourceObject.VisualProperty);
                ndmElementUpdateStrategy.Update(targetObject.NdmElement, sourceObject.NdmElement);
            }
        }

        private void InitializeStrategies()
        {
            point2DUpdateStrategy = new Point2DUpdateStrategy();
            visualPropsUpdateStrategy = new VisualPropsUpdateStrategy();
            ndmElementUpdateStrategy = new NdmElementUpdateStrategy() {UpdateChildren = UpdateChildren };
        }
    }
}
