
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IGetBeamShearSectionIputDatasLogic
    {
        public IBeamShearAction Action { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IBeamShearSection Section { get; set; }
        public IStirrup Stirrup { get; set; }
        public List<IInclinedSection> InclinedSectionList { get; set; }
        List<IBeamShearSectionLogicInputData> GetBeamShearSectionInputDatas();
    }
}