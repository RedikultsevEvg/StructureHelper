using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <summary>
    /// Propertis of concrete in tension
    /// </summary>
    public interface IConcreteFeaTension : ISaveable
    {
        /// <summary>
        /// Strength of concrete in tension, Pa
        /// </summary>
        double Strength { get; set; }
        /// <summary>
        /// Fractur energy, N/m
        /// </summary>
        double FractureEnergy { get; set; }
        /// <summary>
        /// Average size of finish element, m
        /// </summary>
        double FeSize { get; set; }
    }
}
