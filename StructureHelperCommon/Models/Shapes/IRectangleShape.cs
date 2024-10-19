namespace StructureHelperCommon.Models.Shapes
{
    public interface IRectangleShape : IShape
    {
        /// <summary>
        /// Width of rectangle, m
        /// </summary>
        double Width { get; set; }
        /// <summary>
        /// Height of rectangle, m
        /// </summary>
        double Height { get; set; }
    }
}
