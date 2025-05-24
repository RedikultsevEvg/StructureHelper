using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IBeamShearSectionLogicResult : IResult
    {
        IBeamShearSectionLogicInputData InputData { get; set; }
        public double ConcreteStrength { get; set; }
        public double StirrupStrength { get; set; }
        public double TotalStrength { get; set; }
        public double FactorOfUsing { get; }
    }
}
