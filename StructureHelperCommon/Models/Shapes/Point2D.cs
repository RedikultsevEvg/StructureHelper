namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc />
    public class Point2D : IPoint2D
    {
        /// <inheritdoc />
        public double X { get; set; }
        /// <inheritdoc />
        public double Y { get; set; }

        public object Clone()
        {
            var point = new Point2D() { X = X, Y = Y };
            return point;
        }
    }
}
