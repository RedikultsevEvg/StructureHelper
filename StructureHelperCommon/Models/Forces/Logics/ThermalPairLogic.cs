using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces
{
    internal class ThermalPairLogic : IThermalPairLogic
    {
        private IThermalIndependentLogic thermalIndependentLogic;
        public double GetTemperature(IThermalActionTimePair pair, IPoint2D point)
        {
            CheckObject.ThrowIfNull(pair);
            CheckObject.ThrowIfNull(point);
            return thermalIndependentLogic.GetTemperature(pair.ThermalAction, point);
        }
    }
}
