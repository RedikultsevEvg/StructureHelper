using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Forces
{
    internal class GetTemperatureLogic : IGetTemperatureLogic
    {       
        public double GetTemperature(IThermalAction thermalAction, IPoint2D point, double time)
        {
            CheckObject.ThrowIfNull(thermalAction);
            CheckObject.ThrowIfNull(point);
            if (thermalAction is IThermalActionTimeIndependend timeIndependent)
            {
                GetTemperatureByThermalIndependentTimeLogic logic = new();
                return logic.GetTemperature(thermalAction, point, time);
            }
            else if (thermalAction is IThermalActionTimeList timeList)
            {
                ThermalPairTimeInterpolationLogic timeInterpolationLogic = new();
                return timeInterpolationLogic.GetTemperature(timeList.TimePairs, point, time);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(thermalAction) + "thermal action");
            }
        }
    }
}
