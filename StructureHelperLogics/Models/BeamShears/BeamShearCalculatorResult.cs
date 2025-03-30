using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCalculatorResult : IBeamShearCalculatorResult
    {
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public List<IBeamShearSectionLogicResult> SectionResults { get; set; } = new();
    }
}
