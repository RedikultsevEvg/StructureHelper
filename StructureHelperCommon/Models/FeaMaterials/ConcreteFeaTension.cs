using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class ConcreteFeaTension : IConcreteFeaTension
    {
        private double strength = 3.5e6;
        private double fractureEnergy = 72.6;
        private double feSize = 0.001;

        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public double Strength
        {
            get => strength;
            set
            {
                if (value <= 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Strength of concrete must be positive, but was {value}");
                }
                strength = value;
            }
        }
        /// <inheritdoc/>
        public double FractureEnergy
        {
            get => fractureEnergy;
            set
            {
                if (value <= 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Fracture energy must be positive, but was {value}");
                }
                fractureEnergy = value;
            }
        }
        /// <inheritdoc/>
        public double FeSize
        {
            get => feSize;
            set
            {
                if (value <= 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Size of finished element must be positive, but was {value}");
                }
                feSize = value;
            }
        }


        public ConcreteFeaTension() : this(Guid.NewGuid())
        {
            
        }
        public ConcreteFeaTension(Guid id)
        {
            Id = id;
        }
    }
}
