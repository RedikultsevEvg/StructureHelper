using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    /// <inheritdoc/>
    public class EllipseShapeDTO : IEllipseShape
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public double Width { get; set; }
        /// <inheritdoc/>
        public double Height { get; set; }

        public EllipseShapeDTO(Guid id)
        {
            Id = id;
        }
    }
}
