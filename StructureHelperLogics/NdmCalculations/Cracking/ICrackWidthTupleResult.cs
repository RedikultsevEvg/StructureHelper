namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackWidthTupleResult
    {
        double CrackWidth { get; set; }
        bool IsCrackLessThanUltimate { get; }
        double UltimateCrackWidth { get; set; }
    }
}