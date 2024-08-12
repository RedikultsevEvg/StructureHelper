using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Settings for crack calculations assigned by user
    /// </summary>
    public class UserCrackInputData : IUserCrackInputData
    {
        /// <summary>
        /// Flag of assigning of user value of softening factor
        /// </summary>
        public bool SetSofteningFactor { get; set; }
        /// <summary>
        /// User value of softening factor, dimensionless
        /// </summary>
        public double SofteningFactor { get; set; }
        /// <summary>
        /// Flag of assigning of user value of length between cracks
        /// </summary>
        public bool SetLengthBetweenCracks { get; set; }
        /// <summary>
        /// Length between cracks, m
        /// </summary>
        public double LengthBetweenCracks { get; set; }
        /// <summary>
        /// Ultimate long-term crack width, m
        /// </summary>
        public double UltimateLongCrackWidth { get; set; }
        /// <summary>
        /// Ultimate short-term crack width, m
        /// </summary>
        public double UltimateShortCrackWidth { get; set; }
    }
}
