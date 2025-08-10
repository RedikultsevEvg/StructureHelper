using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implements result of calculating of strength of inclined section fore shear
    /// </summary>
    public interface IBeamShearSectionLogicResult : IResult
    {
        /// <summary>
        /// Reference back to input data for calculating
        /// </summary>
        IBeamShearSectionLogicInputData InputData { get; set; }
        /// <summary>
        /// New Input data for calculating
        /// </summary>
        IBeamShearSectionLogicInputData ResultInputData { get; set; }
        /// <summary>
        /// Ultimate shear force due to cocrete
        /// </summary>
        public double ConcreteStrength { get; set; }
        /// <summary>
        /// Ultimate shear force due to stirrups
        /// </summary>
        public double StirrupStrength { get; set; }
        /// <summary>
        /// Total ultimate strength
        /// </summary>
        public double TotalStrength { get; set; }
        /// <summary>
        /// Ration of load to total strength
        /// </summary>
        public double FactorOfUsing { get; }
        /// <summary>
        /// Trace of calculating
        /// </summary>
        public IShiftTraceLogger TraceLogger { get; }
    }
}
