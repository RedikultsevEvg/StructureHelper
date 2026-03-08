using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class CdpProperty : ICdpProperty
    {
        public Guid Id { get; }
        public double DilationAngle { get; set; } = 35.0;
        public double Eccentricity { get; set; } = 0.1;
        public double Fb0Ratio { get; set; } = 1.16;
        public double KRatio { get; set; } = 0.667;
        public double Viscosity { get; set; } = 0.0001;

        public CdpProperty(Guid id)
        {
            Id = id;
        }
    }
}
