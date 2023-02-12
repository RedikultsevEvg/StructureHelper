namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc />
    public class LineShape : ILineShape
    {
        /// <inheritdoc />
        public IPoint2D StartPoint { get; set; }
        /// <inheritdoc />
        public IPoint2D EndPoint { get; set; }
        /// <inheritdoc />
        public double Thickness { get; set; }

        public LineShape()
        {
            StartPoint = new Point2D();
            EndPoint = new Point2D();
            Thickness = 0;
        }
    }
}
