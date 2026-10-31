using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Forces
{
    internal interface IGetTemperatureLogic
    {
        /// <summary>
        /// Calculate temperature at cpecific point and time
        /// </summary>
        /// <param name="point">Point where temperature is found</param>
        /// <param name="time">Time for temperature calculation</param>
        /// <returns>Value of temperature</returns>
        double GetTemperature(IThermalAction thermalAction, IPoint2D point, double time);
    }
}
