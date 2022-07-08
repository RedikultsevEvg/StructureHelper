using System;
using System.Collections.Generic;
using System.Text;
using LoaderCalculator.Data.Materials;

namespace StructureHelperLogics.NdmCalculations.Entities
{
    /// <inheritdoc />
    public class MaterialDecorator : IMaterialDecorator
    {
        /// <inheritdoc />
        public double InitModulus { get; set; }

        /// <inheritdoc />
        public IEnumerable<double> DiagramParameters { get; set; }

        /// <inheritdoc />
        public Func<IEnumerable<double>, double, double> Diagram { get; set; }

        private LoaderCalculator.Data.Materials.IMaterial _material;

        public MaterialDecorator()
        {
            _material = new LoaderCalculator.Data.Materials.Material();
        }

        public LoaderCalculator.Data.Materials.IMaterial GetMaterial()
        {
            return _material;
        }
    }
}
