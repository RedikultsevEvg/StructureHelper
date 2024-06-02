using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarCrackInputData : IInputData
    {
        public IEnumerable<INdm> CrackableNdmCollection { get; set; }
        public IEnumerable<INdm> CrackedNdmCollection { get; set; }
        public ForceTuple ForceTuple { get; set; }
        public double Length { get; set; }
    }
}
