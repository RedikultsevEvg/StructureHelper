namespace StructureHelperLogics.Models.LoadHistory.Logics
{
    /// <summary>
    /// Builds the set of primitives for a construction stage which continues from a previous stage.
    /// </summary>
    public interface INextStagePrimitivesLogic
    {
        /// <summary>
        /// Copies the primitives of the previous stage into <paramref name="stageResult"/> and applies
        /// the prestrain accumulated at the end of the previous stage.
        /// </summary>
        /// <param name="stage">Construction stage which supplies the source primitives.</param>
        /// <param name="stageResult">Result of the stage which receives the prepared primitives.</param>
        /// <param name="previousStageResult">Result of the preceding stage.</param>
        void Process(IConstructionStage stage, IStageResult stageResult, IStageResult previousStageResult);
    }
}
