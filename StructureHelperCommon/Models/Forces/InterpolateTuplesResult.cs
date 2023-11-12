namespace StructureHelperCommon.Models.Forces
{
    public class InterpolateTuplesResult
    {
        public IDesignForceTuple StartTuple { get; set; }
        public IDesignForceTuple FinishTuple { get; set; }
        public int StepCount { get; set; }
    }
}
