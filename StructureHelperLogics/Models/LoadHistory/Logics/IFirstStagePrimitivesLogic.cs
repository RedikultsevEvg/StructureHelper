namespace StructureHelperLogics.Models.LoadHistory.Logics
{
    /// <summary>
    /// Builds the set of primitives for the first construction stage, or for a stage which cannot
    /// inherit state from a predecessor.
    /// </summary>
    public interface IFirstStagePrimitivesLogic
    {
        /// <summary>
        /// Clones the primitives of the stage, clears their automatic prestrain and stores the clones
        /// in <paramref name="stageResult"/>.
        /// </summary>
        /// <param name="stage">Construction stage which supplies the source primitives.</param>
        /// <param name="stageResult">Result of the stage which receives the prepared primitives.</param>
        void Process(IConstructionStage stage, IStageResult stageResult);
    }
}
