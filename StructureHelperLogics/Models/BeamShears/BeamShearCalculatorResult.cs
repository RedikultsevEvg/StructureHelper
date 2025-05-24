using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class BeamShearCalculatorResult : IBeamShearCalculatorResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; } = true;
        /// <inheritdoc/>
        public string? Description { get; set; } = string.Empty;
        /// <inheritdoc/>
        public IBeamShearCalculatorInputData InputData { get; set; }
        /// <inheritdoc/>
        public List<IBeamShearActionResult> ActionResults { get; set; } = new();
    }
}
