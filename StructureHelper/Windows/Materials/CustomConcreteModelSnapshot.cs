using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;

namespace StructureHelper.Windows.ViewModels.Materials
{
    /// <summary>
    /// In-place snapshot of one custom concrete model. It is used to restore the original
    /// values when an editor is cancelled without replacing the model object.
    /// </summary>
    internal sealed class CustomConcreteModelSnapshot
    {
        private readonly ICustomConcreteModel model;

        public string Name { get; }
        public bool IsActive { get; }
        public double YoungModulus { get; }
        public LimitStates LimitState { get; }
        public CalcTerms CalcTerm { get; }

        public double PeakStress { get; }
        public double PeakStrain { get; }
        public double CompressionLimitStrain { get; }
        public double DescendingScaleFactor { get; }

        public double TensionStrength { get; }
        public double UltimateStrain { get; }
        public double TensionLimitStrain { get; }

        public CustomConcreteModelSnapshot(ICustomConcreteModel model)
        {
            this.model = model;
            Name = model.Name;
            IsActive = model.IsActive;
            YoungModulus = model.YoungModulus;
            LimitState = model.StateCalcTermPair.LimitState;
            CalcTerm = model.StateCalcTermPair.CalcTerm;

            PeakStress = model.CompressionOptions.PeakStress;
            PeakStrain = model.CompressionOptions.PeakStrain;
            CompressionLimitStrain = model.CompressionOptions.LimitStrain;
            DescendingScaleFactor = model.CompressionOptions.DescendingScaleFactor;

            TensionStrength = model.TensionOptions.Strength;
            UltimateStrain = model.TensionOptions.UltimateStrain;
            TensionLimitStrain = model.TensionOptions.LimitStrain;
        }

        public void Restore()
        {
            model.Name = Name;
            model.IsActive = IsActive;
            model.YoungModulus = YoungModulus;
            model.StateCalcTermPair.LimitState = LimitState;
            model.StateCalcTermPair.CalcTerm = CalcTerm;

            model.CompressionOptions.PeakStress = PeakStress;
            model.CompressionOptions.PeakStrain = PeakStrain;
            model.CompressionOptions.LimitStrain = CompressionLimitStrain;
            model.CompressionOptions.DescendingScaleFactor = DescendingScaleFactor;

            model.TensionOptions.Strength = TensionStrength;
            model.TensionOptions.UltimateStrain = UltimateStrain;
            model.TensionOptions.LimitStrain = TensionLimitStrain;
        }
    }
}
