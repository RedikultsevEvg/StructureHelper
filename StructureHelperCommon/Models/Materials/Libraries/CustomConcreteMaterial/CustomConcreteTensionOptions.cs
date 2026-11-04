using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    /// <inheritdoc/>
    public class CustomConcreteTensionOptions : ICustomConcreteTensionOptions
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        public double Strength { get; set; } = 1e6;
        /// <inheritdoc/>
        public double UltimateStrain { get; set; } = 0.00015;
        /// <inheritdoc/>
        public double LimitStrain { get; set; } = double.PositiveInfinity;

        /// <inheritdoc/>
        //public double CrackStrain { get; set; } = 0.00015;


        public CustomConcreteTensionOptions(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            CustomConcreteTensionOptions newItem = new(Guid.NewGuid());
            var updateLogic = new CustomConcreteTensionOptionsUpdateStrategy();
            updateLogic.Update(newItem, this);
            return newItem;
        }
    }
}
