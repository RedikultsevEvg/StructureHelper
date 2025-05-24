using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSectionLogicResult : IBeamShearSectionLogicResult
    {
        public bool IsValid { get; set; }
        public string? Description { get; set; }
        public IBeamShearSectionLogicInputData InputData { get; set; }
        public double ConcreteStrength { get; set; }
        public double StirrupStrength { get; set; }
        public double TotalStrength { get; set; }
        public double FactorOfUsing { get => InputData.ForceTuple.Qy / TotalStrength; }
    }
}
