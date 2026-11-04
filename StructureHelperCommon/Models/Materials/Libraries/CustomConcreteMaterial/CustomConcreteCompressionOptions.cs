using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    /// <inheritdoc/>
    public class CustomConcreteCompressionOptions : ICustomConcreteCompressionOptions
    {
        /// <inheritdoc/>
        public Guid Id {  get; }
        /// <inheritdoc/>
        public double PeakStress { get; set; } = 3e7;
        /// <inheritdoc/>
        public double PeakStrain { get; set; } = 0.002;
        /// <inheritdoc/>
        public double LimitStrain { get; set; } = 0.0035;
        /// <inheritdoc/>
        public double DescendingScaleFactor { get; set; } = 1d;


        public CustomConcreteCompressionOptions(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return new CustomConcreteCompressionOptions(Guid.NewGuid())
            {
                PeakStress = PeakStress,
                PeakStrain = PeakStrain,
                LimitStrain = LimitStrain,
                DescendingScaleFactor = DescendingScaleFactor
            };
        }
    }
}
