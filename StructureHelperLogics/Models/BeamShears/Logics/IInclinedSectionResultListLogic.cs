namespace StructureHelperLogics.Models.BeamShears
{
    public interface IInclinedSectionResultListLogic
    {
        List<IBeamShearSectionLogicResult> GetInclinedSectionResults(List<IBeamShearSectionLogicInputData> sectionInputDatas);
    }
}