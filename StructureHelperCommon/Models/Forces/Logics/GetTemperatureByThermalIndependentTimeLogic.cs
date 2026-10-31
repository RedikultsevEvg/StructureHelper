using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Forces
{
    public class GetTemperatureByThermalIndependentTimeLogic : IGetTemperatureLogic
    {
        private IThermalIndependentLogic thermalIndependentLogic;

        public GetTemperatureByThermalIndependentTimeLogic(IThermalIndependentLogic thermalIndependentLogic)
        {
            this.thermalIndependentLogic = thermalIndependentLogic;
        }

        public GetTemperatureByThermalIndependentTimeLogic() : this (new ThermalIndependentLogic()) { }

        /// <inheritdoc/>
        /// <param name="time">Time is ignored because the action is time-independent.</param>
        public double GetTemperature(IThermalAction thermalAction, IPoint2D point, double time)
        {
            CheckObject.ThrowIfNull(thermalAction, nameof(thermalAction));
            CheckObject.ThrowIfNull(point, nameof(point));
            if (thermalAction is not IThermalActionTimeIndependend independend)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(thermalAction));
            }
            return thermalIndependentLogic.GetTemperature(independend, point);
        }
    }
}
