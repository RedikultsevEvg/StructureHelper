using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials
{
    public interface IMaterialLogicOptions
    {
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; }
        public ILibMaterialEntity MaterialEntity { get; set; }
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        bool WorkInCompression { get; set; }
        bool WorkInTension { get; set; }
    }
}
