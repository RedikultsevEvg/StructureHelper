namespace StructureHelperCommon.Models.Forces
{
    public class InterpolateTuplesResult
    {
        public IForceTuple StartTuple { get; set; }
        public IForceTuple FinishTuple { get; set; }
        public int StepCount { get; set; }
    }
}
