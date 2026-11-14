using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
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


        public ICustomConcreteCompressionOptions GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            return new CustomConcreteCompressionOptions(id)
            {
                PeakStress = PeakStress,
                PeakStrain = PeakStrain,
                LimitStrain = LimitStrain,
                DescendingScaleFactor = DescendingScaleFactor
            };
        }
    }
}
