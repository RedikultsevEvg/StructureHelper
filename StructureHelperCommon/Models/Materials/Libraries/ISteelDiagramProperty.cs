namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface ISteelDiagramProperty
    {
        /// <summary>
        /// Limit strain of proportionality, dimensionless
        /// </summary>
        double StrainOfProportionality { get; set; }
        /// <summary>
        /// Strain at point of start of yelding, dimensionless
        /// </summary>
        double StrainOfStartOfYielding { get; set; }
        /// <summary>
        /// Strain at point of end of yelding, dimensionless
        /// </summary>
        double StrainOfEndOfYielding { get; set; }
        /// <summary>
        /// Strain at point of ultimate strength, dimensionless
        /// </summary>
        double StrainOfUltimateStrength { get; set; }
        /// <summary>
        /// Strain at point of fracture
        /// </summary>
        double StrainOfFracture { get; set; }
        /// <summary>
        /// Stress at point of ultimate strength, Pa for absolute value, dimensionless for relative value 
        /// </summary>
        double StressOfUltimateStrength { get; set; }
        /// <summary>
        /// Stress at point of fracture, Pa for absolute value, dimensionless for relative value 
        /// </summary>
        double StressOfFracture { get; set; }
    }
}