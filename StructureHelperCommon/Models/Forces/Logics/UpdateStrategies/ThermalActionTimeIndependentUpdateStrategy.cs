using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Updates a time-independent thermal action. The reference point is updated by the existing
    /// <see cref="Point2DUpdateStrategy"/> when <see cref="UpdateChildren"/> is enabled.
    /// </summary>
    public class ThermalActionTimeIndependentUpdateStrategy : IParentUpdateStrategy<IThermalActionTimeIndependend>
    {
        private readonly IUpdateStrategy<IPoint2D> pointUpdateStrategy;

        public ThermalActionTimeIndependentUpdateStrategy()
            : this(new Point2DUpdateStrategy())
        {
        }

        public ThermalActionTimeIndependentUpdateStrategy(IUpdateStrategy<IPoint2D> pointUpdateStrategy)
        {
            this.pointUpdateStrategy = pointUpdateStrategy;
        }

        public bool UpdateChildren { get; set; } = true;

        public void Update(IThermalActionTimeIndependend targetObject, IThermalActionTimeIndependend sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject))
            {
                return;
            }

            targetObject.RefPointTemperature = sourceObject.RefPointTemperature;
            targetObject.GradientX = sourceObject.GradientX;
            targetObject.GradientY = sourceObject.GradientY;

            if (UpdateChildren)
            {
                CheckObject.ThrowIfNull(sourceObject.ReferencePoint, $"{nameof(sourceObject)}.{nameof(sourceObject.ReferencePoint)}");
                CheckObject.ThrowIfNull(targetObject.ReferencePoint, $"{nameof(targetObject)}.{nameof(targetObject.ReferencePoint)}");
                pointUpdateStrategy.Update(targetObject.ReferencePoint, sourceObject.ReferencePoint);
            }
        }
    }
}
