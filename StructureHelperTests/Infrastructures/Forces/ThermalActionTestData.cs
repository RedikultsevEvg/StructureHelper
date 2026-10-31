using StructureHelperCommon.Models.Forces;

namespace StructureHelperTests.Infrastructures.Forces
{
    /// <summary>
    /// Builds time-independent thermal actions for unit tests.
    /// </summary>
    internal static class ThermalActionTestData
    {
        public static ThermalActionTimeIndependent CreateAction(
            double refPointTemperature,
            double gradientX = 0d,
            double gradientY = 0d,
            double referenceX = 0d,
            double referenceY = 0d)
        {
            return new ThermalActionTimeIndependent
            {
                ReferencePoint = new StructureHelperCommon.Models.Shapes.Point2D(referenceX, referenceY),
                RefPointTemperature = refPointTemperature,
                GradientX = gradientX,
                GradientY = gradientY
            };
        }

        public static ThermalActionTimePair CreatePair(double time, double refPointTemperature)
        {
            return new ThermalActionTimePair(time, CreateAction(refPointTemperature));
        }
    }
}
