using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.States;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Materials.Logics
{
    /// <summary>
    /// Updates one custom concrete model and, when enabled, its state pair and option children.
    /// </summary>
    public class CustomConcreteModelUpdateStrategy : IParentUpdateStrategy<ICustomConcreteModel>
    {
        private readonly IUpdateStrategy<IStateCalcTermPair> stateCalcTermPairUpdateStrategy;
        private readonly IUpdateStrategy<ICustomConcreteCompressionOptions> compressionOptionsUpdateStrategy;
        private readonly IUpdateStrategy<ICustomConcreteTensionOptions> tensionOptionsUpdateStrategy;

        public CustomConcreteModelUpdateStrategy()
            : this(
                new StateCalcTermPairUpdateStrategy(),
                new CustomConcreteCompressionOptionsUpdateStrategy(),
                new CustomConcreteTensionOptionsUpdateStrategy())
        {
        }

        public CustomConcreteModelUpdateStrategy(
            IUpdateStrategy<IStateCalcTermPair> stateCalcTermPairUpdateStrategy,
            IUpdateStrategy<ICustomConcreteCompressionOptions> compressionOptionsUpdateStrategy,
            IUpdateStrategy<ICustomConcreteTensionOptions> tensionOptionsUpdateStrategy)
        {
            this.stateCalcTermPairUpdateStrategy = stateCalcTermPairUpdateStrategy;
            this.compressionOptionsUpdateStrategy = compressionOptionsUpdateStrategy;
            this.tensionOptionsUpdateStrategy = tensionOptionsUpdateStrategy;
        }

        public bool UpdateChildren { get; set; } = true;

        public void Update(ICustomConcreteModel targetObject, ICustomConcreteModel sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject))
            {
                return;
            }

            targetObject.Name = sourceObject.Name;
            targetObject.IsActive = sourceObject.IsActive;
            targetObject.YoungModulus = sourceObject.YoungModulus;

            if (!UpdateChildren)
            {
                return;
            }

            CheckObject.ThrowIfNull(
                sourceObject.StateCalcTermPair,
                $"{nameof(sourceObject)}.{nameof(sourceObject.StateCalcTermPair)}");
            CheckObject.ThrowIfNull(
                targetObject.StateCalcTermPair,
                $"{nameof(targetObject)}.{nameof(targetObject.StateCalcTermPair)}");
            stateCalcTermPairUpdateStrategy.Update(
                targetObject.StateCalcTermPair,
                sourceObject.StateCalcTermPair);

            CheckObject.ThrowIfNull(
                sourceObject.CompressionOptions,
                $"{nameof(sourceObject)}.{nameof(sourceObject.CompressionOptions)}");
            CheckObject.ThrowIfNull(
                targetObject.CompressionOptions,
                $"{nameof(targetObject)}.{nameof(targetObject.CompressionOptions)}");
            compressionOptionsUpdateStrategy.Update(
                targetObject.CompressionOptions,
                sourceObject.CompressionOptions);

            CheckObject.ThrowIfNull(
                sourceObject.TensionOptions,
                $"{nameof(sourceObject)}.{nameof(sourceObject.TensionOptions)}");
            CheckObject.ThrowIfNull(
                targetObject.TensionOptions,
                $"{nameof(targetObject)}.{nameof(targetObject.TensionOptions)}");
            tensionOptionsUpdateStrategy.Update(
                targetObject.TensionOptions,
                sourceObject.TensionOptions);
        }
    }
}
