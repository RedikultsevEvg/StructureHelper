using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials
{
    public class MaterialSettings
    {
        public int Number { get; set; }
        public bool IsActive { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IOneVariableFunction Function { get; set; }
    }
}
