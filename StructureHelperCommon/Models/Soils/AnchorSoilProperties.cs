using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Soils
{
    public class AnchorSoilProperties : IAnchorSoilProperties
    {
        public Guid Id { get;}
        /// <inheritdoc/>
        public double YoungsModulus { get; set; }
        /// <inheritdoc/>
        public double PoissonRatio { get; set; }
        /// <inheritdoc/>
        public double VolumetricWeight { get; set; }
        /// <inheritdoc/>
        public double FrictionAngle { get; set; }
        /// <inheritdoc/>
        public double Coheasion { get; set; }
        /// <inheritdoc/>
        public SoilType SoilType { get; set; }


        public AnchorSoilProperties(Guid id)
        {
            Id = id;
            YoungsModulus = 20e6d; //20MPa
            PoissonRatio = 0.3d;
            VolumetricWeight = 20e3d; //20kN/m^3
            FrictionAngle = 25d;
            Coheasion = 30e3d; //30kPa
            SoilType = SoilType.SandAndSemiSand;
        }
        public AnchorSoilProperties() : this(Guid.NewGuid()) { }
    }
}
