using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implement logic for stirrup bearing capacity which does not depend on specific inclined section
    /// </summary>
    public interface IStirrupByDirectValue : IStirrup
    {
        /// <summary>
        /// Direct value of bearing capacity which is applied for any inclined section, N
        /// </summary>
        double BearingCapacityValue { get; set; }
    }
}
