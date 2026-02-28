using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class ConcreteFeaMaterial : IConcreteFeaMaterial
    {
        private double youngsModulus = 30e9;
        private double poissonsRatio = 0.2;

        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;
        /// <inheritdoc/>
        public double YoungsModulus
        {
            get => youngsModulus;
            set
            {
                if (value <= 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Young's modulus must be positive, but was {value}");
                }
                youngsModulus = value;
            }
        }
        /// <inheritdoc/>
        public double PoissonsRatio
        {
            get => poissonsRatio;
            set
            {
                if (value <= 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Poisson's ratio must be positive, but was {value}");
                }
                poissonsRatio = value;
            }
        }
        /// <inheritdoc/>
        public IConcreteFeaCompression CompressionProperties { get; }
        /// <inheritdoc/>
        public IConcreteFeaTension TensionProperties { get; }

        public ConcreteFeaMaterial(Guid id)
        {
            Id = id;
            CompressionProperties = new ConcreteFeaCompression();
            TensionProperties = new ConcreteFeaTension();
        }
    }
}
