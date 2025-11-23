using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class DeflectionFactor : IDeflectionFactor
    {
        private const double bendCurveFactor = 0.1042;
        private const double longitudinalFactor = 1.0;
        private const double maxDeflection = 0.02;

        public IForceTuple DeflectionFactors { get; set; }
        public double SpanLength { get; set; } = 6.0;
        public IForceTuple MaxDeflections { get; set; }

        public Guid Id { get; }

        public DeflectionFactor(Guid id)
        {
            Id = id;
            DeflectionFactors = new ForceTuple()
            {
                Mx = bendCurveFactor,
                My = bendCurveFactor,
                Mz = longitudinalFactor,
                Nz = longitudinalFactor,
                Qx = longitudinalFactor,
                Qy = longitudinalFactor
            };
            MaxDeflections = new ForceTuple()
            {
                Mx = maxDeflection,
                My = maxDeflection,
                Mz = maxDeflection,
                Nz = maxDeflection,
                Qx = maxDeflection,
                Qy = maxDeflection,
            };
        }
    }
}
