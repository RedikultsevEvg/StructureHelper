namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Implements properties of distributed load with trapezoid distribution of load by length of element
    /// </summary>
    public interface ITrapezoidDistributedLoad : IDistributedBeamSpanLoad
    {
        /// <summary>
        /// Value of trapezoid load at the start point, N/m
        /// </summary>
        IForceTuple StartLoadValue { get; set; }
        /// <summary>
        /// Value of trapezoid load at the start point, N/m
        /// </summary>
        IForceTuple EndLoadValue { get; set; }
    }
}
