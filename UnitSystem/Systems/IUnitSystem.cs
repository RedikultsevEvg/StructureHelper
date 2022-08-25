using System;
using StructureHelper.UnitSystem.Enums;

namespace StructureHelper.UnitSystem.Systems
{
    public interface IUnitSystem
    {
        string Name { get; }
        Tuple<Force, MultiplyPrefix> ForceUnits { get; }
        Tuple<Pressure, MultiplyPrefix> PressureUnits { get; }
        Tuple<Length, MultiplyPrefix> LengthUnits { get; }
        double ConvertLength(double length);
    }
}