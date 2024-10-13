using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class MaterialPartialFactorDTO : IMaterialPartialFactor
    {
        public Guid Id { get; set; }
        public double FactorValue { get; set; }
        public StressStates StressState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public LimitStates LimitState { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
