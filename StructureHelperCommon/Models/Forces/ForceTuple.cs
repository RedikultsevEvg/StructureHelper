using System.ComponentModel.DataAnnotations;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ForceTuple : IForceTuple
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
            IForceTuple forceTuple = new ForceTuple() { Mx = Mx, My = My, Nz = Nz, Qx = Qx, Qy = Qy, Mz = Mz};
            return forceTuple;
        }
        public static ForceTuple operator +(ForceTuple first) => first;
        public static ForceTuple operator +(ForceTuple first, ForceTuple second)
        {
            var result = new ForceTuple();
            result.Mx += first.Mx + second.Mx;
            result.My += first.My + second.My;
            result.Mz += first.Mz + second.Mz;
            result.Qx += first.Qx + second.Qx;
            result.Qy += first.Qy + second.Qy;
            result.Nz += first.Nz + second.Nz;
            return result;
        }
    }
}
