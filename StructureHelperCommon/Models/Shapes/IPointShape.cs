namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Geomentry primitive of point
    /// </summary>
    public interface IPointShape : IShape
    {
        /// <summary>
        /// Area of point
        /// </summary>
        double Area { get; set; }
    }
}
