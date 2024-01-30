using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Soils
{
    public class AnchorResult : IResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string? Description { get; set; }
        /// <summary>
        /// Characteristic Bearing Capacity, N
        /// </summary>
        public double CharBearingCapacity { get; set; }
        /// <summary>
        /// Design Bearing Capacity, N
        /// </summary>
        public double DesignBearingCapacity { get; set; }
        /// <summary>
        /// Average side pressure, Pa
        /// </summary>
        public double AverageSidePressure { get;set; }
        /// <summary>
        /// Volume of mortar of first stady, m^3
        /// </summary>
        public double MortarVolumeFstStady { get; set; }
        /// <summary>
        /// Volume of mortar of second stady, m^3
        /// </summary>
        public double MortarVolumeSndStady { get; set; }
        public double AnchorCenterLevel { get; set; }

    }
}
