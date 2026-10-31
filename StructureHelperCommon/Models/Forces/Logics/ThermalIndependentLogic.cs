using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Forces
{
    public class ThermalIndependentLogic : IThermalIndependentLogic
    {
        public double GetTemperature(IThermalActionTimeIndependend action, IPoint2D point)
        {
            CheckObject.ThrowIfNull(action, nameof(action));
            CheckObject.ThrowIfNull(point, nameof(point));
            IPoint2D referencePoint = action.ReferencePoint;
            CheckObject.ThrowIfNull(referencePoint, $"{nameof(action)}.{nameof(action.ReferencePoint)}");

            return action.RefPointTemperature
                + action.GradientX * (point.X - referencePoint.X)
                + action.GradientY * (point.Y - referencePoint.Y);
        }
    }
}
