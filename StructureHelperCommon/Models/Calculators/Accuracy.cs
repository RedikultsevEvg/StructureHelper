using System;

namespace StructureHelperCommon.Models.Calculators
{
    /// <inheritdoc/>
    public class Accuracy : IAccuracy
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public double IterationAccuracy { get; set; }
        /// <inheritdoc/>
        public int MaxIterationCount { get; set; }
        public Accuracy(Guid id)
        {
            Id = id;
        }

        public Accuracy() : this (Guid.NewGuid())
        {
            
        }

    }
}
