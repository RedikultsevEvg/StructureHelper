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
    public class TupleCrackInputData : IInputData
    {
        public bool IsValid { get; set; }
        public IForceTuple? LongTermTuple { get; set; }
        public IForceTuple? ShortTermTuple { get; set; }
        public List<INdmPrimitive>? NdmPrimitives {get;set;}
        public UserCrackInputData UserCrackInputData { get; set; }
    }
}
