using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <summary>
    /// Properties of concrete in tension
    /// </summary>
    public interface IConcreteFeaCompression : ISaveable
    {
        /// <summary>
        /// Strength of concrete in compression (positive), Pa
        /// </summary>
        double Strength { get; set; }
        /// <summary>
        /// Strain in  ultimate stress point, dimensionless
        /// </summary>
        double PeakStrain { get; set; }
        /// <summary>
        /// Ratio of elastic stress to ultimate stress 
        /// </summary>
        double ElasticStressRatio { get; set; }
        /// <summary>
        /// Value of scaling of descending branch of concrete's stress-strain curve
        /// </summary>
        double DescendingScaleFactor { get; set; }
    }
}
