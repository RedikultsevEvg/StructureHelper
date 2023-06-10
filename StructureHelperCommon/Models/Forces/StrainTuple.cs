using StructureHelperCommon.Services.Forces;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class StrainTuple : IForceTuple
    {
        /// <inheritdoc/>
        public double Mx { get; set; }
        /// <inheritdoc/>
        public double My { get; set; }
        /// <inheritdoc/>
        public double Nz { get; set; }
        /// <inheritdoc/>
        public double Qx { get; set; }
        /// <inheritdoc/>
        public double Qy { get; set; }
        /// <inheritdoc/>
        public double Mz { get; set; }

        /// <inheritdoc/>
        public object Clone()
        {
            StrainTuple forceTuple = new StrainTuple() { Mx = Mx, My = My, Nz = Nz, Qx = Qx, Qy = Qy, Mz = Mz };
            return forceTuple;
        }
        public static StrainTuple operator +(StrainTuple first) => first;
        public static StrainTuple operator +(StrainTuple first, ForceTuple second)
        {
            return ForceTupleService.SumTuples(first, second) as StrainTuple;
        }
    }
}
