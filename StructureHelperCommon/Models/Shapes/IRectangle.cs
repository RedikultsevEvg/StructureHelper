namespace StructureHelperCommon.Models.Shapes
{
    public interface IRectangle : IShape
    {
        /// <summary>
        /// Width of rectangle, m
        /// </summary>
        double Width { get; }
        /// <summary>
        /// Height of rectangle, m
        /// </summary>
        double Height { get; }
        /// <summary>
        /// Angle of rotating rectangle, rad
        /// </summary>
        double Angle { get; }
    }
}
