using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public class MaterialLogicOption : IMaterialLogicOptions
    {
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; }
        public ILibMaterialEntity MaterialEntity { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public bool WorkInCompression { get; set; } = true;
        public bool WorkInTension { get; set; } = true;
    }
}
