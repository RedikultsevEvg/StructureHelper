using StructureHelperCommon.Infrastructures.Interfaces;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implement properties of stirrups
    /// </summary>
    public interface IStirrup : ISaveable, ICloneable
    {
        string? Name { get; set; }
        /// <summary>
        /// Distance from axis of comressed rebar to edge of comressed zone, m 
        /// </summary>
        double CompressedGap { get; set; }
    }
}
