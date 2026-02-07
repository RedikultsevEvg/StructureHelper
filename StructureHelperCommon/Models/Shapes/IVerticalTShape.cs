namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Implements properties for T-shape
    /// </summary>
    public interface IVerticalTShape : IShape
    {
        /// <summary>
        /// Full height of t-shape, m
        /// </summary>
        double FullHeight { get; set; }
        /// <summary>
        /// Width of web, m
        /// </summary>
        double WebWidth { get; set; }
        /// <summary>
        /// Height of flange, m
        /// </summary>
        double FlangeHeight { get; set; }
        /// <summary>
        /// Width of flange, m
        /// </summary>
        double FlangeWidth { get; set; }
        /// <summary>
        /// Distance of Offset of flange along x-axis
        /// </summary>
        double FlangeXOffset { get; set; }
    }
}
