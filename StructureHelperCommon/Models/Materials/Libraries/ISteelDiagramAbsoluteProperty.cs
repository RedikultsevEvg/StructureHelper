using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface ISteelDiagramAbsoluteProperty : ISteelDiagramProperty
    {
        /// <summary>
        /// Initial modulus of elasticity (Young's modulus), Pa
        /// </summary>
        double InitialYoungsModulus { get; set; }
        /// <summary>
        /// Stress of yelding, Pa
        /// </summary>
        double BaseStrength { get; set; }
        /// <summary>
        /// Strain of start of yielding under bilinear diagram
        /// </summary>
        double BaseStrain { get; set; }
        /// <summary>
        /// Stress at point of limit of proportionality, Pa
        /// </summary>
        double StressOfProportionality { get; set; }
    }
}
