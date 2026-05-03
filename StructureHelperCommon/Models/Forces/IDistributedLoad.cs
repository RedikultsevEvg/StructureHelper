namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Implement properties of uniformly distributed beam span load
    /// </summary>
    public interface IDistributedLoad : IDistributedBeamSpanLoad
    {
        /// <summary>
        /// Value of uniformly distributed load, N/m
        /// </summary>
        IForceTuple LoadValue { get; set; }
    }
}