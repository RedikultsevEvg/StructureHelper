using System;
using StructureHelper.UnitSystem.Enums;
using StructureHelper.UnitSystem.Systems;

namespace StructureHelper.UnitSystem
{
    internal class SystemSi : IUnitSystem
    {
        public SystemSi()
        {
            LengthUnits = new Tuple<Length, MultiplyPrefix>(Length.M, MultiplyPrefix.m);
        }

        public string Name => "СИ";
        public Tuple<Force, MultiplyPrefix> ForceUnits { get; }
        public Tuple<Pressure, MultiplyPrefix> PressureUnits { get; }
        public Tuple<Length, MultiplyPrefix> LengthUnits { get; }
        public double ConvertLength(double length)
        {
            switch (LengthUnits.Item2)
            {
                case MultiplyPrefix.m:
                    return length / 1000;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
