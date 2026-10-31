using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.Forces
{
    internal interface IThermalPairLogic
    {
        double GetTemperature(IThermalActionTimePair pair, IPoint2D point);
    }
}