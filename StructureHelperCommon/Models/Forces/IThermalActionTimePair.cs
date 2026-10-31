namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Pair of a time moment and a temperature state which is defined at that moment.
    /// </summary>
    public interface IThermalActionTimePair
    {
        /// <summary>
        /// Time of the state, in seconds
        /// </summary>
        double Time { get; set; }

        /// <summary>
        /// Temperature state at <see cref="Time"/>
        /// </summary>
        IThermalActionTimeIndependend ThermalAction { get; set; }
    }
}
