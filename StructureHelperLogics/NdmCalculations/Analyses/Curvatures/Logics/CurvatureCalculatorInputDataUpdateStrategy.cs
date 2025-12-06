using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    /// <summary>
    /// Provides an update strategy for <see cref="ICurvatureCalculatorInputData"/>,
    /// allowing transfer of child collections and nested objects from a source
    /// instance to a target instance.
    /// </summary>
    public class CurvatureCalculatorInputDataUpdateStrategy
        : IParentUpdateStrategy<ICurvatureCalculatorInputData>
    {
        private IUpdateStrategy<IDeflectionFactor>? _deflectionUpdateStrategy;

        /// <summary>
        /// Gets the strategy used to update the <see cref="IDeflectionFactor"/> object.
        /// Lazily initialized to <see cref="DeflectionFactorUpdateStrategy"/> if none supplied.
        /// </summary>
        private IUpdateStrategy<IDeflectionFactor> DeflectionUpdateStrategy =>
            _deflectionUpdateStrategy ??= new DeflectionFactorUpdateStrategy();

        /// <summary>
        /// When true, the children's data (collections and nested objects) 
        /// will be updated from the source. Defaults to true.
        /// </summary>
        public bool UpdateChildren { get; set; } = true;

        /// <summary>
        /// Creates a new instance of the update strategy.
        /// </summary>
        /// <param name="deflectionUpdateStrategy">
        /// Optional custom update strategy for <see cref="IDeflectionFactor"/>.
        /// </param>
        public CurvatureCalculatorInputDataUpdateStrategy(
            IUpdateStrategy<IDeflectionFactor>? deflectionUpdateStrategy = null)
        {
            _deflectionUpdateStrategy = deflectionUpdateStrategy;
        }

        /// <inheritdoc />
        public void Update(ICurvatureCalculatorInputData target, ICurvatureCalculatorInputData source)
        {
            CheckObject.ThrowIfNull(source, nameof(source));
            CheckObject.ThrowIfNull(target, nameof(target));

            if (ReferenceEquals(target, source))
                return;
            target.ConsiderSofteningFactor = source.ConsiderSofteningFactor;
            if (UpdateChildren)
            {
                ValidateChildProperties(target, source);
                UpdateCollections(target, source);
                DeflectionUpdateStrategy.Update(target.DeflectionFactor, source.DeflectionFactor);
            }
        }

        /// <summary>
        /// Validates that all required child properties are not null.
        /// </summary>
        private static void ValidateChildProperties(
            ICurvatureCalculatorInputData target,
            ICurvatureCalculatorInputData source)
        {
            CheckObject.ThrowIfNull(source.Primitives, nameof(source.Primitives));
            CheckObject.ThrowIfNull(target.Primitives, nameof(target.Primitives));

            CheckObject.ThrowIfNull(source.ForceActions, nameof(source.ForceActions));
            CheckObject.ThrowIfNull(target.ForceActions, nameof(target.ForceActions));

            CheckObject.ThrowIfNull(source.DeflectionFactor, nameof(source.DeflectionFactor));
            CheckObject.ThrowIfNull(target.DeflectionFactor, nameof(target.DeflectionFactor));
        }

        /// <summary>
        /// Replaces the target collections with copies of the source collections.
        /// </summary>
        private static void UpdateCollections(
            ICurvatureCalculatorInputData target,
            ICurvatureCalculatorInputData source)
        {
            target.Primitives.Clear();
            target.Primitives.AddRange(source.Primitives);

            target.ForceActions.Clear();
            target.ForceActions.AddRange(source.ForceActions);
        }
    }

}
