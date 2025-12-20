using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class SteelMaterialLogicOption : ISteelMaterialLogicOption
    {
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; }
        public ILibMaterialEntity MaterialEntity { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public bool WorkInCompression { get; set; } = true;
        public bool WorkInTension { get; set; } = true;
        public double MaxPlasticStrainRatio { get; set; }
        public double UlsFactor { get; set; }
        public double SlsFactor { get; set; }
        public double ThicknessFactor { get; set; }
        public double WorkConditionFactor { get; set; }
    }
}
