using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Provides a temperature which does not depend on time: the value is set at a reference point
    /// and changes linearly along the local X and Y axes.
    /// Temperatures are expressed in kelvins, gradients in kelvins per metre.
    /// </summary>
    public interface IThermalActionTimeIndependend : IThermalAction
    {
        /// <summary>
        /// Point where the temperature is assigned
        /// </summary>
        IPoint2D ReferencePoint { get; }

        /// <summary>
        /// Temperature at the reference point, in kelvins
        /// </summary>
        double RefPointTemperature { get; set; }

        /// <summary>
        /// Gradient of temperature along the X axis, in kelvins per metre
        /// </summary>
        double GradientX { get; set; }

        /// <summary>
        /// Gradient of temperature along the Y axis, in kelvins per metre
        /// </summary>
        double GradientY { get; set; }
    }
}
