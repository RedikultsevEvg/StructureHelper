using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Converters;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface ICdpProperty : ISaveable
    {
        /// <summary>
        /// Angle of dilation, degree
        /// </summary>
        double DilationAngle { get; set; }
        double Eccentricity { get; set; }
        double Fb0Ratio { get; set; }
        double KRatio { get; set; }
        double Viscosity {  get; set; }
    }
}
