using StructureHelperCommon.Infrastructures.Exceptions;
using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class ConcreteFeaCompression : IConcreteFeaCompression
    {
        private const double mimimumElasticRatio = 0.2;
        private double strength = 4.0e7;
        private double strainOfUltimateStress = 0.0022;
        private double elasticStressRatio = 0.4;

        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public double Strength
        {
            get => strength;
            set
            {
                //if (value <= 0)
                //{
                //    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Strength of concrete must be positive, but was {value}");
                //}
                strength = value;
            }
        }
        /// <inheritdoc/>
        public double PeakStrain
        {
            get => strainOfUltimateStress;
            set
            {
                //if (value <= 0)
                //{
                //    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Strain of concrete must be positive, but was {value}");
                //}
                strainOfUltimateStress = value;
            }
        }
        /// <inheritdoc/>
        public double ElasticStressRatio
        {
            get => elasticStressRatio;
            set
            {
                //if (value <= mimimumElasticRatio)
                //{
                //    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Ratio of elastic stress to ultimate stress must be greater than minimum value {mimimumElasticRatio}, but was {value}");
                //}
                elasticStressRatio = value;
            }
        }

        public double DescendingScaleFactor { get; set; } = 1.0;

        public ConcreteFeaCompression() : this (Guid.NewGuid())
        {
            
        }
        public ConcreteFeaCompression(Guid id)
        {
            Id = id;
        }
    }
}
