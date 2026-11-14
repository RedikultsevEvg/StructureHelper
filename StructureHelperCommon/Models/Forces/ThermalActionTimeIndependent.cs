using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Thermal action whose temperature field does not change with time.
    /// The temperature is a linear function of the point coordinates:
    /// <c>T(x, y) = RefPointTemperature + GradientX * (x - x0) + GradientY * (y - y0)</c>,
    /// where (x0, y0) is <see cref="ReferencePoint"/>.
    /// Temperatures are expressed in kelvins, gradients in kelvins per metre.
    /// </summary>
    public class ThermalActionTimeIndependent : IThermalActionTimeIndependend
    {
        
        private IParentUpdateStrategy<IThermalActionTimeIndependend> updateStrategy;

        /// <inheritdoc/>
        public Guid Id { get; }


        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc/>
        public IPoint2D ReferencePoint { get; set; } = new Point2D();

        /// <inheritdoc/>
        public double RefPointTemperature { get; set; } = 293.15d;

        /// <inheritdoc/>
        public double GradientX { get; set; }

        /// <inheritdoc/>
        public double GradientY { get; set; }
        public ThermalActionTimeIndependent(Guid id)
        {
            Id = id;
        }

        public ThermalActionTimeIndependent() : this (Guid.NewGuid())
        {
            
        }

        public IThermalActionTimeIndependend GetClone(IThermalActionTimeIndependend sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject;}
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            updateStrategy ??= new ThermalActionTimeIndependentUpdateStrategy() { UpdateChildren = false };
            ThermalActionTimeIndependent clone = new(id)
            {
                Name = Name
            };
            updateStrategy.Update(clone, this);
            clone.ReferencePoint = sourceObject.ReferencePoint.GetClone(sourceObject.ReferencePoint, cloneMode, cloneDepth); //always create deep clone of point
            return clone;
        }
    }
}
