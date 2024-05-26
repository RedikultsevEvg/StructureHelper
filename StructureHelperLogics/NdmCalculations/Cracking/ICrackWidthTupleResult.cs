namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackWidthTupleResult
    {
        /// <summary>
        /// Calculated crack width
        /// </summary>
        double CrackWidth { get; set; }
        bool IsCrackLessThanUltimate { get; }
        double UltimateCrackWidth { get; set; }
    }
}