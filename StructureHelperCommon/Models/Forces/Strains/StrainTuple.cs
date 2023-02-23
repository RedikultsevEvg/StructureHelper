using StructureHelperCommon.Services.Forces;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class StrainTuple : IStrainTuple
    {
        /// <inheritdoc/>
        public double Kx { get; set; }
        /// <inheritdoc/>
        public double Ky { get; set; }
        /// <inheritdoc/>
        public double EpsZ { get; set; }
        /// <inheritdoc/>
        public double Gx { get; set; }
        /// <inheritdoc/>
        public double Gy { get; set; }
        /// <inheritdoc/>
        public double Gz { get; set; }

        /// <inheritdoc/>
        public object Clone()
        {
            var target = new StrainTuple();
            StrainTupleService.CopyProperties(this, target);
            return target;
        }
    }
}
